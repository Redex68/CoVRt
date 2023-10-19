using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ControlPanelServer: MonoBehaviour
{
    [SerializeField] int port = 6942;

    public GameEvent serverFound;
    public GameEvent serverDisconnected;
    public static string data = "";

    void Start()
    {
        StartCoroutine(ReadText());
    }

    IEnumerator ReadText()
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:{port}/"))
        {
            while(true)
            {
                yield return www.SendWebRequest();

                if(www.result == UnityWebRequest.Result.ConnectionError)
                {
                    if(data != "") 
                    {
                        serverDisconnected.SimpleRaise();
                        data = "";
                    }
                    yield return new WaitForSeconds(1);
                }
                else if (www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.DataProcessingError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    if(data == "")
                    {
                        serverFound.SimpleRaise();
                    }
                    data = www.downloadHandler.text;
                }
            }
        }
    }
}