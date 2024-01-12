using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputSwitcher : MonoBehaviour
{
    [SerializeField] ControlPanel controlPanel;
    [SerializeField] CameraSelector cameraSelector;
    [SerializeField] List<Dial> dials;
    [SerializeField] List<Toggle> floors;
    public void OnServerFound()
    {
        controlPanel.enabled = true;
        cameraSelector.enabled = true;
        foreach(Dial dial in dials) dial.UIInteractionEnabled = false;
        foreach(Toggle toggle in floors) toggle.interactable = false;
    }

    public void OnServerDisconnected()
    {
        controlPanel.enabled = false;
        cameraSelector.enabled = false;
        foreach(Dial dial in dials) dial.UIInteractionEnabled = true;
        foreach(Toggle toggle in floors) toggle.interactable = true;
    }
}