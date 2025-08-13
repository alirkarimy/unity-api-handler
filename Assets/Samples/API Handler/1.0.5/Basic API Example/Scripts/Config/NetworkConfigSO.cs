using UnityEngine;

[CreateAssetMenu(fileName = "NetworkConfigSO", menuName = "Network/NetworkConfigSO")]
public class NetworkConfigSO : ScriptableObject
{
    public string BASE_URL = "https://puzzle-api.booaligames.ir/v1/api/app";
    public string APP_VERSION = "1.0.6";
    public string OS_VERSION = "1";
    public string OS_NAME = "android";
    public string FIREBASE_TOKEN = "token";
}
