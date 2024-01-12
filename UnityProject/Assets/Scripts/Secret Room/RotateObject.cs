using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public enum Axis { X, Y, Z }
    [SerializeField]
    public Axis axis;
    /// <summary> The number of degrees by which the object should rotate. Must be positive. </summary>
    public float angle;
    /// <summary> The speed at which the object will rotate at. Can be negative to change rotation direction. </summary>
    public float angularSpeed;

    public void Rotate()
    {
        StartCoroutine(DoRotation());
    }
    

    private IEnumerator DoRotation()
    {
        float angleLeft = angle;

        while(angleLeft > 0.0f)
        {
            float rotation = Mathf.Min(angleLeft, angularSpeed * Time.deltaTime);
            angleLeft -= Mathf.Abs(rotation);
            RotateBy(axis, rotation);
            yield return new WaitForEndOfFrameUnit();
        }
    }

    private void RotateBy(Axis ax, float an)
    {
        switch(ax)
        {
            case Axis.X:
                transform.eulerAngles += Vector3.right * an;
                break;
            case Axis.Y:
                transform.eulerAngles += Vector3.up * an;
                break;
            case Axis.Z:
                transform.eulerAngles += Vector3.forward * an;
                break;
        }
    }
}
