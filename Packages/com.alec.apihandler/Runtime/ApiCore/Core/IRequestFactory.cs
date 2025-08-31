using Best.HTTP;
using UnityEngine.Networking;

namespace Alec.Core
{
    public interface IRequestFactory
    {
        HTTPRequest Post<T>(IPostRequest<T> req) where T : Response;
        HTTPRequest Get<T>(IGetRequest<T> req) where T : Response;
        HTTPRequest Put<T>(IPutRequest<T> req) where T : Response;
        HTTPRequest Delete<T>(IDeleteRequest<T> req) where T : Response;
    }
}
