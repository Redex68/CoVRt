using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverviewMap : MonoBehaviour
{
    private int layer;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        layer = 6;
    }

    public void SwitchFloor(int floor)
    {
        if (floor >= 1 && floor <= 5) layer = floor + 5;
        else layer = 6;
        cam.cullingMask = (1 << layer) | (1 << LayerMask.NameToLayer("Stairs")) | (1 << LayerMask.NameToLayer("MapIcon"));
    }
}
