/* This camera is supposed to detect whenever a guard passes in front. 
It raycasts and It needs to be able to configure the FOV.
It should not go through walls
It should have an option to visualize the FOV with gizmos
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycasterCamera : MonoBehaviour
{
    public Camera cameraComponent;

    public bool castRays = true;
    public LayerMask raycastLayerMask = -1;
    public float rayLength = 100f;

    public int rows = 10;
    public int cols = 10;

    private void OnDrawGizmos()
    {
        if (cameraComponent == null)
            cameraComponent = GetComponent<Camera>();

        if (cameraComponent != null)
        {
            Vector3 cameraPosition = cameraComponent.transform.position; // Store the camera's position.

            float near = cameraComponent.nearClipPlane;
            float far = cameraComponent.farClipPlane;
            float fov = cameraComponent.fieldOfView;
            float aspect = cameraComponent.aspect;

            float heightNear = 2 * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad) * near;
            float widthNear = heightNear * aspect;

            float heightFar = 2 * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad) * far;
            float widthFar = heightFar * aspect;

            //Vector3 nearTopLeft = new Vector3(-widthNear / 2, heightNear / 2, near);
            //Vector3 nearTopRight = new Vector3(widthNear / 2, heightNear / 2, near);
            //Vector3 nearBottomLeft = new Vector3(-widthNear / 2, -heightNear / 2, near);
            //Vector3 nearBottomRight = new Vector3(widthNear / 2, -heightNear / 2, near);

            Vector3 farTopLeft = new Vector3(-widthFar / 2, heightFar / 2, far);
            Vector3 farTopRight = new Vector3(widthFar / 2, heightFar / 2, far);
            Vector3 farBottomLeft = new Vector3(-widthFar / 2, -heightFar / 2, far);
            Vector3 farBottomRight = new Vector3(widthFar / 2, -heightFar / 2, far);

            farTopLeft = cameraComponent.transform.rotation * farTopLeft + cameraPosition;
            farTopRight = cameraComponent.transform.rotation * farTopRight + cameraPosition;
            farBottomLeft = cameraComponent.transform.rotation * farBottomLeft + cameraPosition;
            farBottomRight = cameraComponent.transform.rotation * farBottomRight + cameraPosition;

            Debug.DrawLine(cameraPosition, farTopLeft, Color.magenta);
            Debug.DrawLine(cameraPosition, farTopRight, Color.magenta);
            Debug.DrawLine(cameraPosition, farBottomRight, Color.magenta);
            Debug.DrawLine(cameraPosition, farBottomLeft, Color.magenta);

            if (castRays)
            {
                //CastRaysFromFrustum1(cameraPosition, farTopLeft, farTopRight, farBottomLeft, farBottomRight);
                CastRaysFromFrustum2(cameraPosition, new Vector3[] { farTopLeft, farTopRight, farBottomLeft, farBottomRight }, rows, cols);
            }
        }
    }

    private void CastRaysFromFrustum1(Vector3 cameraPosition, params Vector3[] corners)
    {


        foreach (Vector3 corner in corners)
        {
            Vector3 worldPos = corner;

            Vector3 direction = worldPos - cameraPosition;

            if (Physics.Raycast(cameraPosition, direction.normalized, out RaycastHit hit, rayLength, raycastLayerMask))
            {
                Debug.DrawLine(cameraPosition, hit.point, Color.red);
            }
            else
            {
                Debug.DrawLine(cameraPosition, cameraPosition + direction.normalized * rayLength, Color.blue);
            }
        }
    }

    // cast rays in a grid from the camera to the far plane
    private void CastRaysFromFrustum2(Vector3 cameraPosition, Vector3[] corners, int rows, int columns)
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


                if (Physics.Raycast(cameraPosition, worldPos - cameraPosition, out RaycastHit hit, rayLength, raycastLayerMask))
                {
                    Debug.DrawLine(cameraPosition, hit.point, Color.red);
                }
                else
                {
                    Debug.DrawLine(cameraPosition, worldPos, Color.green);
                }


            }
        }
    }

}