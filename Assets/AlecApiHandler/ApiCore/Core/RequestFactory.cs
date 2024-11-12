using Alec.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Alec.Core
{
    class RequestFactory : IRequestFactory
    {
        public UnityWebRequest Get<T>(IGetRequest<T> req) where T : Response
        {
            return UnityWebRequest.Get(UrlUtils.EncodeGetUrl(UrlUtils.BuildUrl(req), req.Params));
        }

        public UnityWebRequest Post<T>(IPostRequest<T> req) where T : Response
        {
            return UnityWebRequest.Post(UrlUtils.BuildUrl(req), req.FormData == null?default(Dictionary<string,string>):req.FormData);
        }

        public UnityWebRequest Put<T>(IPutRequest<T> req) where T : Response
        {
            UnityWebRequest putReq = UnityWebRequest.Put(UrlUtils.BuildUrl(req), Encoding.UTF8.GetBytes(req.JSONData ?? ""));
            putReq.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
            return putReq;
        }
    }
}
