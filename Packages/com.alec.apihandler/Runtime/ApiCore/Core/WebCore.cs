using Alec.Api;
using Best.HTTP;
using Best.HTTP.Request.Upload;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alec.Core
{
    public class WebCore
    {
        private static Dictionary<string, string> HEADERS = new Dictionary<string, string>();
        private static MonoBehaviour _mono;
        public static event Action OnTokenExpired;

        private static float rateLimitTime = 1;
        private static float lastRequestSendTime = float.MinValue;
        private static int rateLimitExceededCount = 1;
        private static bool _isInitialized => _mono != null && HEADERS.Count > 0;

        #region Headers


        /// <summary>
        /// Set HEADERS for ongoing requests
        /// </summary>
        /// <param name="headers">It is a list of key value pairs contaiting headers inforamtions</param>

        public static bool Initialize(MonoBehaviour mono, string baseUrl, Dictionary<string, string> headers)
        {
            if (mono == null) return false;
            if (string.IsNullOrWhiteSpace(baseUrl)) return false;
            if (headers == null || headers.Count == 0) return false;

            _mono = mono;
            UrlUtils.SetBaseUrl(baseUrl);
            HEADERS = headers;

            return true; // IMPORTANT: this only means configured, not "internet ok"
        }

        public static void AddOrUpdateHeaders(Dictionary<string, string> headers)
        {
            if (headers == null || headers.Count == 0) return;
            if (HEADERS == null) HEADERS = new Dictionary<string, string>();

            foreach (var kv in headers)
                HEADERS[kv.Key] = kv.Value;
        }

        #endregion


        public static void Post<T>(IPostRequest<T> req) where T : Response
        {
            HTTPRequest request = new RequestFactory().Post<T>(req);

            /*if (string.IsNullOrEmpty(req.JSONData) == false) */
            setRequestBody(req.JSONData, request);

            SendRequest(req, request);

        }

        public static void Get<T>(IGetRequest<T> req) where T : Response
        {
            HTTPRequest request = new RequestFactory().Get<T>(req);
            SendRequest(req, request);

        }

        public static void Put<T>(IPutRequest<T> req) where T : Response
        {
            HTTPRequest request = new RequestFactory().Put<T>(req);

            SendRequest(req, request);

        }

        public static void Delete<T>(IDeleteRequest<T> req) where T : Response
        {
            HTTPRequest request = new RequestFactory().Delete<T>(req);

            /*if (string.IsNullOrEmpty(req.JSONData) == false) */
            setRequestBody(req.JSONData, request);

            SendRequest(req, request);

        }

        private static void SendRequest<T>(IRequest<T> req, HTTPRequest request) where T : Response
        {
            if (!_isInitialized)
            {
                Debug.LogError("You must initialize AlecNetwork first");
                return;
            }
            _mono.StartCoroutine(Send(req, request));

        }
        private static IEnumerator Send<T>(IRequest<T> req, HTTPRequest request) where T : Response
        {
            float waitTime = (rateLimitTime * rateLimitExceededCount + 0.2f) - (Time.time - lastRequestSendTime);
            if (waitTime > 0)
                yield return new WaitForSeconds(rateLimitExceededCount++ * rateLimitTime - (Time.time - lastRequestSendTime));

            rateLimitExceededCount = Mathf.Clamp(rateLimitExceededCount - 1, 1, int.MaxValue);
            lastRequestSendTime = Time.time;

            SetRequestHeaders(request);

            Debug.Log(DataUtils.GetCurlCommand(request, HEADERS));

            // Helper to respond with your ErrorResponse
            void ReplyError(ResponseStatus status, string[] error, string msg)
            {
                var errorReply = new ErrorResponse(status, error, msg);
                req.OnResponse(JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(errorReply)));
            }

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.Log("No internet connection detected before request.");

                ReplyError(ResponseStatus.INTERNET_ERROR, new string[] { "no_internet" }, "No internet connection");

                yield break; // STOP coroutine immediately
            }

            yield return request.Send();

            // ✅ Response can be null on DNS failure / unplugged network
            var resp = request.Response;
            var ex = request.Exception;

            switch (request.State)
            {
                case HTTPRequestStates.Finished:
                    {
                        // In Finished, Response is usually not null, but still guard anyway
                        if (resp != null && resp.IsSuccess)
                        {
                            var bytes = resp.Data ?? Array.Empty<byte>();
                            var responseJSON = System.Text.Encoding.UTF8.GetString(bytes);
                            Debug.Log($"response json : {responseJSON}");
                            try
                            {
                                var responseObject = JsonConvert.DeserializeObject<T>(responseJSON);

                                if (responseObject.Status == ResponseStatus.NOT_AUTHENTICATED)
                                    OnTokenExpired?.Invoke();
                                else
                                    req.OnResponse(responseObject);
                            }
                            catch
                            {
                                ReplyError(ResponseStatus.MODEL_MISMATCH_ERR, new string[] { "failed_to_deserialize" }, $"Failed to deserialize {typeof(T)}");
                            }
                        }
                        else
                        {
                            // Finished but not success => server responded with error OR malformed response
                            if (resp == null)
                            {
                                // Rare, but possible
                                string message;

                                if (DataUtils.IsDnsError(ex))
                                    message = "DNS error: Could not resolve host";
                                else
                                    message = ex?.Message ?? "Network error";

                                ReplyError(ResponseStatus.INTERNET_ERROR, new string[] { "server_no_response" }, ex?.Message ?? "No response received");
                                break;
                            }

                            Debug.Log($"Server sent an error: {resp.StatusCode}-{resp.Message}");
                            Debug.Log($"Http Error {ex}");


                            // Try parse server error body if any
                            try
                            {
                                var bytes = resp.Data ?? Array.Empty<byte>();
                                var responseJSON = System.Text.Encoding.UTF8.GetString(bytes);
                                Debug.Log($"Response Body:\n{responseJSON}");
                                Debug.Log($"Response Headers:\n{DataUtils.ConvertToJSON(resp.Headers)}");
                                var errorRes = JsonConvert.DeserializeObject<T>(responseJSON);
                                if (errorRes.Status == ResponseStatus.NOT_AUTHENTICATED)
                                {
                                    OnTokenExpired?.Invoke();
                                }
                                ReplyError(errorRes.Status, errorRes.Error, errorRes.Message ?? "HTTP error");
                            }
                            catch
                            {
                                ReplyError(ResponseStatus.HTTP_ERROR, new string[] { "failed_to_deserialize_error" }, resp.Message ?? "HTTP error (no body)");
                            }
                        }


                        break;
                    }

                case HTTPRequestStates.TimedOut:
                case HTTPRequestStates.ConnectionTimedOut:
                    Debug.Log("Network Error: ConnectionTimedOut");
                    ReplyError(ResponseStatus.INTERNET_ERROR, new string[] { "connection_timeout" }, "ConnectionTimedOut");
                    break;

                case HTTPRequestStates.Error:
                case HTTPRequestStates.Aborted:
                default:
                    {
                        // ✅ This is the unplugged internet / DNS fail lane
                        Debug.Log($"Request failed. State={request.State}, Exception={ex}");


                        // Map common network failures
                        string msg = ex?.Message ?? "Network error (no response)";

                        // If you want to detect DNS specifically:
                        if (ex is System.Net.Sockets.SocketException se)
                            msg = $"SocketException ({se.ErrorCode}): {se.Message}";

                        ReplyError(ResponseStatus.INTERNET_ERROR, new string[] { "request_aborted" }, msg);
                        break;

                    }
            }
        }


        /// <summary>
        /// Set a string to body of the request. 
        /// It has to be serialized by expecting format from server before sending as a parameter.
        /// </summary>
        /// <param name="postData"> usually is in JSON format</param>
        /// <param name="request"> current proccessing request</param>
        private static void setRequestBody(string postData, HTTPRequest request)
        {
            //byte[] bodyRaw = Encoding.UTF8.GetBytes(postData ?? "");
            request.UploadSettings.UploadStream = new JSonDataStream(postData);
        }


        /// <summary>
        /// Set current HEADERS to a Http request 
        /// </summary>
        /// <param name="request"> request is a UnityWebRequest object</param>
        private static void SetRequestHeaders(HTTPRequest request)
        {
            if (HEADERS == null || HEADERS.Count == 0) return;

            foreach (KeyValuePair<string, string> header in HEADERS)
            {
                request.SetHeader(header.Key, header.Value);
            }
        }

    }


}
