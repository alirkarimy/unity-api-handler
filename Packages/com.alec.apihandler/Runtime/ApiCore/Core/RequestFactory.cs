using Best.HTTP;
using Best.HTTP.Request.Upload;
using Best.HTTP.Request.Upload.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace Alec.Core
{
    class RequestFactory : IRequestFactory
    {
        public HTTPRequest Delete<T>(IDeleteRequest<T> req) where T : Response
        {
            // 1. Create request with a callback
            HTTPRequest request = new HTTPRequest(UrlUtils.BuildUrl(req),HTTPMethods.Delete);
            return request;
        }

        public HTTPRequest Get<T>(IGetRequest<T> req) where T : Response
        {
            // 1. Create request with a callback
            var request = HTTPRequest.CreateGet(UrlUtils.EncodeGetUrl(req, req.Params));

            return request;
        }

        public HTTPRequest Post<T>(IPostRequest<T> req) where T : Response
        {
            // 1. Create request with a callback
            var request = HTTPRequest.CreatePost(UrlUtils.BuildUrl(req));

            // 2. Setup request parameters
            if(req.FormData != null)
            {
                MultipartFormDataStream formData = new MultipartFormDataStream();
                foreach (var field in req.FormData)
                {
                    formData.AddField(field.Key, field.Value);
                }
                request.UploadSettings.UploadStream = formData;
            }
            

            return request;
        }

        public HTTPRequest Put<T>(IPutRequest<T> req) where T : Response
        {
            HTTPRequest request = new HTTPRequest(UrlUtils.BuildUrl(req), HTTPMethods.Put);
            request.UploadSettings.UploadStream = new JSonDataStream(req.JSONData);
            return request;
        }
    }
}
