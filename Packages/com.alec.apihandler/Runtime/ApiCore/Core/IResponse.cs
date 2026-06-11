using Alec.Api;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Alec.Core
{

    /// <summary>
    /// Holds the data came from a network request.
    /// </summary>
    /// <typeparam name="T">Type of response body. e.g. if registering a user, this would be UserRegisterModel</typeparam>
    public interface IResponse<out T> : IResponse
    {
        /// <summary>
        /// Content of the response, converted to type specified by T
        /// </summary>
        T Body { get; }
    }

    public interface IResponse
    {
        /// <summary>
        ///  Response status code received from Alecgames server which generally show what
        //   has happened in processing request.
        //   Cast this to Alec.ApiCore.ResponseStatus  to get the corresponding
        //   enum.
        /// </summary>
        ResponseStatus Status { get; }

        /// <summary>
        /// If a request was unsuccessful, this shows the cause of error happened in the process.
        /// nullable: true
        /// </summary>
        string Message { get; }

        DateTime Date { get; }

        int Code { get; }
        /// <summary>
        /// Not Ok response message
        /// nullable: true
        /// </summary>
        string[] Error { get; }

    }

    [Serializable]
    public class Response<T> : Response, IResponse<T>
    {
        public T data { set; get; }

        public T Body => data;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(Body, Formatting.Indented);
        }
    }

    [Serializable]
    public class Response : IResponse
    {
        public DateTime date { set; get; }
        public int code { set; get; }
        public string status { set; get; }
        public string message { set; get; }

        [JsonProperty("error")]
        public string[] error { set; get; }

        public DateTime Date => date;
        public int Code => code;
        public ResponseStatus Status
        {
            get
            {
                try
                {
                    ResponseStatus parsed = (ResponseStatus)(code);
                    return parsed;
                }
                catch (Exception ex) { return ResponseStatus.UNKNOWN_ERROR; }
            }
        }
        public string Message => message;

        public string[] Error => error;
    }


}


