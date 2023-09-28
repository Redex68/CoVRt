/* This camera is supposed to detect whenever a guard passes in front. 
It raycasts and It needs to be able to configure the FOV.
It should not go through walls
It should have an option to visualize the FOV with gizmos
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycasterCamera : MonoBehaviour
{
    public Camera cameraComponent;

    public bool castRays = true;
    public LayerMask raycastLayerMask = -1;
    public float lengthPercentage = 100f;

    public int rows = 10;
    public int cols = 10;

    public float updateInterval = 0.5f;

    // reference to the ui icon for the camera
    public Toggle cameraIcon = null;

    private void Start()
    {

        // get global camera in the scene
        if (cameraComponent == null)
            cameraComponent = Camera.main;

        StartCoroutine(CastRaysCoroutine());

    }
    private IEnumerator CastRaysCoroutine()
    {
        while (true)
        {
            CastRays();
            yield return new WaitForSeconds(updateInterval);
        }
    }

    private void CastRays()
    {
        // reset color
        if (cameraIcon != null)
            cameraIcon.transform.localScale = new Vector3(1f, 1f, 1f);


        if (cameraComponent != null)
        {

            // TODO: Move this to start
            Vector3 cameraPosition = transform.position; // Store the camera's position.

            float near = cameraComponent.nearClipPlane;
            float far = cameraComponent.farClipPlane;
            float fov = cameraComponent.fieldOfView;
            float aspect = cameraComponent.aspect;

            float heightNear = 2 * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad) * near;
            float widthNear = heightNear * aspect;

            float heightFar = 2 * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad) * far;
            float widthFar = heightFar * aspect;

            Vector3 farTopLeft = new Vector3(-widthFar / 2, heightFar / 2, far);
            Vector3 farTopRight = new Vector3(widthFar / 2, heightFar / 2, far);
            Vector3 farBottomLeft = new Vector3(-widthFar / 2, -heightFar / 2, far);
            Vector3 farBottomRight = new Vector3(widthFar / 2, -heightFar / 2, far);

            farTopLeft = transform.rotation * farTopLeft + cameraPosition;
            farTopRight = transform.rotation * farTopRight + cameraPosition;
            farBottomLeft = transform.rotation * farBottomLeft + cameraPosition;
            farBottomRight = transform.rotation * farBottomRight + cameraPosition;

            //Debug.DrawLine(cameraPosition, farTopLeft, Color.magenta);
            //Debug.DrawLine(cameraPosition, farTopRight, Color.magenta);
            //Debug.DrawLine(cameraPosition, farBottomRight, Color.magenta);
            //Debug.DrawLine(cameraPosition, farBottomLeft, Color.magenta);

            if (castRays)
            {
                CastRaysFromFrustum(cameraPosition, new Vector3[] { farTopLeft, farTopRight, farBottomLeft, farBottomRight }, rows, cols);
            }
        }
    }


    // cast rays in a grid from the camera to the far plane
    private void CastRaysFromFrustum(Vector3 cameraPosition, Vector3[] corners, int rows, int columns)
    {

        // Calculate the direction from the camera to the corners of the far plane.
        Vector3 topLeft = corners[0];
        Vector3 topRight = corners[1];
        Vector3 bottomLeft = corners[2];
        Vector3 bottomRight = corners[3];

        // Iterate through the grid of points on the far plane.
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {

                Vector3 worldPos = Vector3.Lerp(Vector3.Lerp(topLeft, topRight, (float)col / (columns - 1)), Vector3.Lerp(bottomLeft, bottomRight, (float)col / (columns - 1)), (float)row / (rows - 1));
                //Debug.DrawLine(cameraPosition, worldPos, Color.blue);


                // Take into account the length percentage
                Vector3 finalPosition = cameraPosition + (worldPos - cameraPosition) * lengthPercentage / 100f;

                // if hit's a guard (layer)
                if (Physics.Raycast(cameraPosition, finalPosition, out RaycastHit hit, lengthPercentage, raycastLayerMask))
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Guard"))
                    {
                        Debug.DrawLine(cameraPosition, hit.point, Color.red);
                        if (cameraIcon != null && !cameraIcon.isOn)
                            cameraIcon.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                        //Debug.Log("hit");
                        break;

                    }
                    else
                    {
                        Debug.DrawLine(cameraPosition, hit.point, Color.green);
                    }
                }
                else // if it doesn't hit anything
                {
                    Debug.DrawLine(cameraPosition, finalPosition, Color.green);
                }
            }
        }
    }

}