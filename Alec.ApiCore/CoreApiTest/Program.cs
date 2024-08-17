using System;
using Elka.ApiCore;
using UnityEngine;

namespace CoreApiTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            GameObject gameObject = new GameObject();
            LoginApi loginApi = new LoginApi();
            Mono mono =  gameObject.AddComponent<Mono>();
            loginApi.Send(mono);

        }
    }
}
