using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSpotting : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    FloatVariable guardFOV;
    [SerializeField]
    GameEvent playerSpotted;

    // Start is called before the first frame update
    void Start()
    {
        if(player == null) Debug.LogError("No Player GameObject set");
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
        
        if(player != null && !State.gameOver)
        {
            if(PlayerIsInFront())
            {
                //TODO: Make a better check for whether the ray hit the player (tag?)
                //TODO: Add layermask
                if (Physics.Raycast(transform.position, player.transform.position - transform.position, out RaycastHit hitInfo) && hitInfo.transform.gameObject.name == "Player")
                {
                    Debug.Log("In LOS");
                    playerSpotted.Raise(this, null);
                    State.gameOver = true;
                }
            }
        }
    }

    private bool PlayerIsInFront() {
        return Vector3.Angle(transform.forward, player.transform.position - transform.position) < guardFOV.Value / 2.0f;
    }
}
