using Alec.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Alec.Core
{
    public interface IRequestFactory
    {
        UnityWebRequest Post<T>(IPostRequest<T> req) where T : Response;
        UnityWebRequest Get<T>(IGetRequest<T> req) where T : Response;
        UnityWebRequest Put<T>(IPutRequest<T> req) where T : Response;
    }
}
