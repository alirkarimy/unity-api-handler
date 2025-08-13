using Alec.Api;
using UnityEngine;

public class AlecApiHandlerTest : MonoBehaviour
{
    [SerializeField] NetworkConfigSO configSO;
    void Awake()
    {
        AlecListener.WebCoreInitialized += () => { Debug.Log("WebCore Initialized"); };
        AlecListener.WebCoreInitializeFailed += (error) => { Debug.Log("WebCore Initialize Failed : " + error); };
        AlecListener.OnSignupFailed += (status, message) => { Debug.Log(status + message); };
        AlecListener.OnSignup += (userDatamodel) => { AlecNetwork.GetUserProfile(); };
        AlecListener.OnUserProfileFailed += (status,message) => { Debug.Log(status + message); };
        AlecListener.OnUserProfileRecived += (userData) => { Debug.Log(userData); };
        AlecListener.OnTokenExpired +=()=> { Debug.Log("User Token Expired"); };

    }
    // Start is called before the first frame update
    public void ConnectToServer()
    {
        AlecNetwork.Initialize(configSO.BASE_URL, "device-uuid", configSO.APP_VERSION, this);
    }
    public void SendSampleAPI()
    {
        AlecNetwork.SignupAsGuest("Ali");
    }

}
