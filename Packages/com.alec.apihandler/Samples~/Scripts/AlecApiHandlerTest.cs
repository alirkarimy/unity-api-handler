using Alec.Api;
using UnityEngine;

public class AlecApiHandlerTest : MonoBehaviour
{
    void Awake()
    {
        AlecListener.WebCoreInitialized += () => { Debug.Log("WebCore Initialized"); };
        AlecListener.WebCoreInitializeFailed += (error) => { Debug.Log("WebCore Initialize Failed : " + error); };
    }
    // Start is called before the first frame update
    public void ConnectToServer()
    {
        AlecNetwork.Initialize("BaseURL", "ClientID", "ClientSecret", "DeviceUUID", "StoreName", "ApkVersion", "OsVersion", "OsName", this);
    }
    public void SendSampleAPI()
    {
        AlecNetwork.GetAppConfig("ConnectionData");
    }
   
}
