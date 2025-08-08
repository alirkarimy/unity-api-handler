using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace Alec.Core
{
    class RequestFactory : IRequestFactory
    {
        public UnityWebRequest Delete<T>(IDeleteRequest<T> req) where T : Response
        {
            return UnityWebRequest.Delete(UrlUtils.BuildUrl(req));
        }

        public UnityWebRequest Get<T>(IGetRequest<T> req) where T : Response
        {
            return UnityWebRequest.Get(UrlUtils.EncodeGetUrl(req, req.Params));
        }

        public UnityWebRequest Post<T>(IPostRequest<T> req) where T : Response
        {
            return UnityWebRequest.Post(UrlUtils.BuildUrl(req), req.FormData == null?default(Dictionary<string,string>):req.FormData);
        }

        public UnityWebRequest Put<T>(IPutRequest<T> req) where T : Response
        {
            return UnityWebRequest.Put(UrlUtils.BuildUrl(req), Encoding.UTF8.GetBytes(req.JSONData ?? ""));
        }
    }
}
