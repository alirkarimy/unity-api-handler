using System.Collections.Generic;
using UnityEngine;

namespace Alec.Core
{

    public interface IPutRequest<T> : IRequest<T>, IPutRequestData where T : IResponse
    {

    }
    public interface IGetRequest<T> : IRequest<T>, IRequestGetData where T : IResponse
    {

    }
    public interface IPostRequest<T> : IRequest<T>, IRequestPostData where T : IResponse
    {

    }
    public interface IDeleteRequest<T> : IRequest<T>, IDeleteRequestData where T : IResponse
    {

    }

    /// <summary>
    /// Requests with a callback for handling responses
    /// </summary>
    /// <typeparam name="T"> T must be IResponse</typeparam>
    public interface IRequest<T> : IRequest where T : IResponse
    {
        void OnResponse(T result);


    }
    /// <summary>
    /// Base Request with capablity of sending requests to specific URL
    /// </summary>
    public interface IRequest
    {
        string Route { get; }

        string ApiVersion { get; }


    }
    public interface IRequestPostData
    {
        string JSONData { set; get; }
        Dictionary<string, string> FormData { set; get; }
    }

    public interface IRequestGetData
    {
        Dictionary<string, string> Params { set; get; }
    }

    public interface IPutRequestData
    {
        string JSONData { set; get; }
    }

    public interface IDeleteRequestData
    {
        string JSONData { set; get; }
    }



}
