using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;

    // Below is not my code. This stuff is just for testing out the 3D sounds. 
    // Source: https://www.youtube.com/watch?v=5Rq8A4H6Nzw 

    // Variables
    public Transform player;
    public float mouseSensitivity = 2f;
    float cameraVerticalRotation = 0f;

    bool lockedCursor = true;


    void Start()
    {
        // Lock and Hide the Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }


    void Update()
    {
        // Collect Mouse Input

        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the Camera around its local X axis

        cameraVerticalRotation -= inputY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;


        // Rotate the Player Object and the Camera around its Y axis

        player.Rotate(Vector3.up * inputX);


        // Not copied code:

        // Move player with WASD
        Vector3 mov = player.forward * Input.GetAxis("Vertical") + player.right * Input.GetAxis("Horizontal");
        controller.Move(mov * 0.1f);

        StudioEventEmitter em = transform.GetComponent<StudioEventEmitter>();
        if (!em.IsPlaying() && mov != Vector3.zero) em.Play();
        else if (mov == Vector3.zero) em.Stop();

        //if (Input.GetMouseButtonDown(0)) em.Play();
        //else if (Input.GetMouseButtonDown(1)) RuntimeManager.PlayOneShot(ev, transform.position);
    }
}
