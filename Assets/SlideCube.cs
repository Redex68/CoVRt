using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlideCube : MonoBehaviour
{
    [SerializeField] Transform startPos, endPos;
    [SerializeField] Lever lever;
    
    // Start is called before the first frame update
    void Start()
    {
        lever.leverUpdate += OnLeverUpdate;
    }

    private void OnLeverUpdate(object sender, LeverEventArgs e)
    {
        transform.position = Vector3.Lerp(startPos.position, endPos.position, e.value);
    }
}