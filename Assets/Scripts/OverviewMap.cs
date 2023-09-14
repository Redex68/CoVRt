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
        //if (floor >= 1 && floor <= 5) layer = floor + 5;
        //else layer = 6;
        //cam.cullingMask = (1 << layer) | (1 << LayerMask.NameToLayer("Stairs")) | (1 << LayerMask.NameToLayer("MapIcon"));

        // for floor 1
        if (floor == 1)
        {
            cam.nearClipPlane = 95;
            cam.farClipPlane = 105;
        }
        // for floor 2
        else if (floor == 2)
        {
            cam.nearClipPlane = 85;
            cam.farClipPlane = 95;
        }
    }
}
