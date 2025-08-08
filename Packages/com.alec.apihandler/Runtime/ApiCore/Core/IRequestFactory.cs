using UnityEngine.Networking;

namespace Alec.Core
{
    public interface IRequestFactory
    {
        UnityWebRequest Post<T>(IPostRequest<T> req) where T : Response;
        UnityWebRequest Get<T>(IGetRequest<T> req) where T : Response;
        UnityWebRequest Put<T>(IPutRequest<T> req) where T : Response;
        UnityWebRequest Delete<T>(IDeleteRequest<T> req) where T : Response;
    }
}
