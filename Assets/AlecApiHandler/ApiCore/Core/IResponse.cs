using Alec.Api;
using Alec.Utils;
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
        /// </summary>
        string Message { get; }

    }

    [Serializable]
    public class Response<T> :Response, IResponse<T>
    {
        [SerializeField] protected T data;

        public T Body => data;
    }

    [Serializable]
    public class Response : IResponse
    {
        [SerializeField] protected string message;
        [SerializeField] protected bool success;
        [SerializeField] protected int error_code;

        public ResponseStatus Status { get { try { return success ? ResponseStatus.Successful : (ResponseStatus)error_code; } catch (Exception ex) { return ResponseStatus.UNKNOWN_ERROR; } } }

        public string Message => message;
    }

}


