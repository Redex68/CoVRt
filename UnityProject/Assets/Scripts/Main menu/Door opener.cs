using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorOpener : MonoBehaviour
{
    [SerializeField]
    float moveAmount = 3.0f;
    [SerializeField]
    float moveSpeed = 3.0f;

    public void OpenDoor()
    {
        StartCoroutine(MoveDoor());
    }

    private IEnumerator MoveDoor()
    {
        float movementLeft = moveAmount;

        while(movementLeft > 0.0f)
        {
            float movement = Mathf.Min(movementLeft, moveSpeed * Time.deltaTime);
            movementLeft -= movement;
            transform.localPosition += Vector3.right * movement;
            yield return new WaitForEndOfFrameUnit();
        }
    }
}
