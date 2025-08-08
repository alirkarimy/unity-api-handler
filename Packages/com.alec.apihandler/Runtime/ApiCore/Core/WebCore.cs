using Alec.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Alec.Core
{
    public class WebCore
    {
        private static Dictionary<string, string> HEADERS = new Dictionary<string, string>();
        private static MonoBehaviour _mono;
        public static event Action OnTokenExpired;

        private static bool _isInitialized => _mono != null && HEADERS.Count > 0;

        #region Headers


        /// <summary>
        /// Set HEADERS for ongoing requests
        /// </summary>
        /// <param name="headers">It is a list of key value pairs contaiting headers inforamtions</param>
        public static bool Initialize(MonoBehaviour mono, string baseUrl, Dictionary<string, string> headers)
        {
            _mono = mono;
            UrlUtils.SetBaseUrl(baseUrl);
            HEADERS = headers;
            return true;
        }


        public static void AddOrUpdateHeaders(Dictionary<string, string> headers)
        {
            foreach (KeyValuePair<string, string> header in headers)
            {
                if (HEADERS.ContainsKey(header.Key))
                    HEADERS[header.Key] = header.Value;
                else
                    HEADERS.Add(header.Key, header.Value);
            }
        }


        #endregion


        public static void Post<T>(IPostRequest<T> req) where T : Response
        {
            UnityWebRequest request = new RequestFactory().Post<T>(req);

            /*if (string.IsNullOrEmpty(req.JSONData) == false) */
            setRequestBody(req.JSONData, request);

            SendRequest(req, request);

        }

        public static void Get<T>(IGetRequest<T> req) where T : Response
        {
            UnityWebRequest request = new RequestFactory().Get<T>(req);
            SendRequest(req, request);

        }

        public static void Put<T>(IPutRequest<T> req) where T : Response
        {
            UnityWebRequest request = new RequestFactory().Put<T>(req);

            SendRequest(req, request);

        }

        public static void Delete<T>(IDeleteRequest<T> req) where T : Response
        {
            UnityWebRequest request = new RequestFactory().Delete<T>(req);

            /*if (string.IsNullOrEmpty(req.JSONData) == false) */
            setRequestBody(req.JSONData, request);

            SendRequest(req, request);

        }

        private static void SendRequest<T>(IRequest<T> req, UnityWebRequest request) where T : Response
        {
            if (!_isInitialized)
            {
                Debug.LogError("You must initialize AlecNetwork first");
                return;
            }
            _mono.StartCoroutine(Send(req, request));

        }
        private static IEnumerator Send<T>(IRequest<T> req, UnityWebRequest request) where T : Response
        {

            SetRequestHeaders(request);

            Debug.Log(DataUtils.GetCurlCommand(request, HEADERS));
            request.timeout = 20;
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                // TODO : Invoke callback with network error code
                // Consider sharing this reason with the user to guide their troubleshooting actions.
                Debug.Log($"Network Error {request.error}");
                ErrorResponse error = new ErrorResponse(ResponseStatus.INTERNET_ERROR, request.error);

                req.OnResponse(JsonUtility.FromJson<T>(JsonUtility.ToJson(error)));
            }
            else if (request.result == UnityWebRequest.Result.DataProcessingError)
            {
                // TODO : Invoke callback with network error code
                // Consider sharing this reason with the user to guide their troubleshooting actions.
                Debug.Log($"DataProcessingError Error {request.error}");
                ErrorResponse error = new ErrorResponse(ResponseStatus.DataProcessingError, request.error);

                req.OnResponse(JsonUtility.FromJson<T>(JsonUtility.ToJson(error)));
            }
            else if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                // TODO : Invoke callback with http error code
                // Consider sharing this reason with the user to guide their troubleshooting actions.
                Debug.Log($"Http Error {request.error}");

                if (request.responseCode == 401)
                    OnTokenExpired?.Invoke();
                else
                {

                    try
                    {
                        //Debugger.Log("Successful with code : " + request.responseCode);
                        byte[] result = request.downloadHandler.data;
                        string responseJSON = System.Text.Encoding.Default.GetString(result);
                        //  Debug.Log($"response json : {responseJSON}");
                        T responseObject = null;
                        responseObject = JsonUtility.FromJson<T>(responseJSON);
                        Debug.Log($"Response Body:\n{responseJSON}");
                        Debug.Log($"Response Headers:\n{DataUtils.ConvertToJSON(request.GetResponseHeaders())}");
                        ErrorResponse error = new ErrorResponse(ResponseStatus.HTTP_ERROR, request.error);
                        req.OnResponse(JsonUtility.FromJson<T>(JsonUtility.ToJson(error)));
                    }
                    catch (Exception ex)
                    {
                        Debug.Log($"catched Http Error not encapsulated by server");

                        ErrorResponse error = new ErrorResponse(ResponseStatus.HTTP_ERROR, request.error);
                        req.OnResponse(JsonUtility.FromJson<T>(JsonUtility.ToJson(error)));
                    }


                }
            }
            else
            {
                //Debugger.Log("Successful with code : " + request.responseCode);
                byte[] result = request.downloadHandler.data;
                string responseJSON = System.Text.Encoding.Default.GetString(result);
                //  Debug.Log($"response json : {responseJSON}");
                T responseObject = null;
                try
                {
                    responseObject = JsonUtility.FromJson<T>(responseJSON);
                }
                catch (Exception ex)
                {
                    ErrorResponse error = new ErrorResponse(ResponseStatus.MODEL_MISMATCH_ERR, $"Failed to deserialize {typeof(T).ToString()}");
                    responseObject = JsonUtility.FromJson<T>(JsonUtility.ToJson(error));
                }

                if (responseObject.Status == ResponseStatus.NOT_AUTHENTICATED)
                    OnTokenExpired?.Invoke();
                else
                {
                    Debug.Log($"Response Body:\n{responseJSON}");
                    Debug.Log($"Response Headers:\n{DataUtils.ConvertToJSON(request.GetResponseHeaders())}");
                    req.OnResponse(responseObject);

                }

            }
            request?.Dispose();

        }


        /// <summary>
        /// Set a string to body of the request. 
        /// It has to be serialized by expecting format from server before sending as a parameter.
        /// </summary>
        /// <param name="postData"> usually is in JSON format</param>
        /// <param name="request"> current proccessing request</param>
        private static void setRequestBody(string postData, UnityWebRequest request)
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(postData ?? "");
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        }


        /// <summary>
        /// Set current HEADERS to a Http request 
        /// </summary>
        /// <param name="request"> request is a UnityWebRequest object</param>
        private static void SetRequestHeaders(UnityWebRequest request)
        {
            if (HEADERS == null || HEADERS.Count == 0) return;

            foreach (KeyValuePair<string, string> header in HEADERS)
            {
                request.SetRequestHeader(header.Key, header.Value);
            }
        }

    }


}
