using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideCube : MonoBehaviour
{
    [SerializeField] Transform startPos, endPos;
    [SerializeField] Lever lever;

    // Start is called before the first frame update
    void Start()
    {
        lever.leverUpdate += OnUpdate;
    }

    private void OnUpdate(object sender, LeverEventArgs e)
    {
        Debug.Log(e.value);
        transform.position = Vector3.Lerp(startPos.position, endPos.position, e.value);
    }
}
