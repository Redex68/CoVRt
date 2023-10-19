using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ControlPanelServerMock))]
public class ControlPanelServerMockEditor: Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ControlPanelServerMock mock = (ControlPanelServerMock) target;

        if(GUILayout.Button("Server found"))
        {
            mock.serverFound.SimpleRaise();
        }
        if(GUILayout.Button("Server disconnected"))
        {
            mock.serverDisconnected.SimpleRaise();
        }
    }
}