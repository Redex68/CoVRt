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
        while(true)
        {
            using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:{port}/data"))
            {
                //Debug.Log("Start of Read");
                yield return www.SendWebRequest();

                if(www.result == UnityWebRequest.Result.ConnectionError)
                {
                    if(data != "") Disconnected();
                    www.Abort();
                    yield return new WaitForSeconds(1);
                }
                else if (www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.DataProcessingError)
                {
                    www.Abort();
                    Debug.Log(www.error);
                }
                else
                {
                    if(data == "") Connected();
                    data = www.downloadHandler.text;
                }
            }
        }
    }

    private void Disconnected()
    {
        Debug.Log("Disconnected from control panel server");
        serverDisconnected.SimpleRaise();
        data = "";
    }

    private void Connected()
    {
        Debug.Log("Connected to control panel server");
        serverFound.SimpleRaise();
    }
}