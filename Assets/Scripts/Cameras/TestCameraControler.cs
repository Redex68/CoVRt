using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestCameraControler : MonoBehaviour, CameraControler
{
    private UnityEvent onClick = new UnityEvent();

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            onClick?.Invoke();
        }
    }

    public void AddSelectListener(UnityAction action)
    {
        onClick.AddListener(action);
    }

    public Vector2 GetDirection()
    {
        Vector2 position = Input.mousePosition;
        if(position.x < Screen.width / 2 || position.y < Screen.height / 2)
            return Vector2.zero;
        return new Vector2(Mathf.InverseLerp(Screen.width / 2, Screen.width, position.x) * 2.0f - 1.0f, Mathf.InverseLerp(Screen.height / 2, Screen.height, position.y) * 2.0f - 1.0f);
    }
}
