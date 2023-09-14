using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SpottablesList;

public class GuardSpotting : MonoBehaviour
{
    [SerializeField]
    FloatVariable guardFOV;
    [SerializeField]
    GameEvent playerSpotted;
    [SerializeField]
    GameEvent playerEnteredLOS;
    [SerializeField]
    /// <summary> A list of all game objects that can be spotted by the guard. </summary>
    SpottablesList spottableObjects;
    [SerializeField]
    /// <summary> The layers the spot raycast can hit </summary>
    LayerMask mask;

    private float spotMeter = 0.0f;
    private bool spotting = false;

    // Start is called before the first frame update
    void Start()
    {
        if(spottableObjects.spottables.Count == 0) Debug.LogError("No spottable objects in list.");
    }

    // Update is called once per frame
    void Update()
    {
        //Show raycast
        //if (Physics.Raycast(transform.position, player.transform.position - transform.position, out RaycastHit hitInfoTest) && hitInfoTest.transform.gameObject.name == "Player")
        //{
        //    if(PlayerIsInFront())
        //        Debug.DrawLine(transform.position, hitInfoTest.point, Color.green);
        //    else
        //        Debug.DrawLine(transform.position, hitInfoTest.point, Color.blue);
        //}
        //else
        //    Debug.DrawLine(transform.position, hitInfoTest.point, Color.red);


        //Show FOV in Scene editor
        //Debug.DrawLine(transform.position, Vector3.RotateTowards(transform.forward, transform.right, guardFOV.Value / 2.0f / 180f * Mathf.PI, 0) * 100 + transform.position);
        //Debug.DrawLine(transform.position, Vector3.RotateTowards(transform.forward, -transform.right,guardFOV.Value / 2.0f / 180f * Mathf.PI, 0) * 100 + transform.position);
        
        float highestSpotCoef = 0.0f;

        if(!State.gameOver)
        {
            //Find the object with the fastest spot time that's visible to the guard.
            foreach (SpottableObject spottable in spottableObjects.spottables)
            {
                if(ObjectIsInFront(spottable.obj)
                    && Physics.Raycast(transform.position, spottable.obj.transform.position - transform.position, out RaycastHit hitInfo, 300.0f, mask)
                    && hitInfo.transform.gameObject == spottable.obj
                )
                {
                    highestSpotCoef = highestSpotCoef == 0.0f ? spottable.spotTime : Mathf.Min(highestSpotCoef, spottable.spotTime);
                    if(!spotting)
                    {
                        playerEnteredLOS.Raise(this, null);
                        spotting = true;
                    }
                }
            }
        }

        if(highestSpotCoef == 0.0f)
        {
            spotting = false;
            spotMeter = 0.0f;
        }
        else spotMeter += Time.deltaTime / highestSpotCoef;

        Debug.Log("Spot meter: " + spotMeter);
        if(spotMeter >= 1.0f)
        {
            Debug.Log("Player spotted by \"" + name + "\"");
            playerSpotted.Raise(this, null);
            State.gameOver = true;
        }
    }

    private bool ObjectIsInFront(GameObject obj) {
        return Vector3.Angle(transform.forward, obj.transform.position - transform.position) < guardFOV.Value / 2.0f;
    }
}
