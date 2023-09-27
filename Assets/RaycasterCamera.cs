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

    private void OnDrawGizmos()
    {
        if (cameraComponent == null)
            cameraComponent = GetComponent<Camera>();

        if (cameraComponent != null)
        {
            Vector3 cameraPosition = cameraComponent.transform.position; // Store the camera's position.

            Gizmos.color = Color.yellow;
            Matrix4x4 matrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

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

            //Gizmos.DrawLine(nearTopLeft, nearTopRight);
            //Gizmos.DrawLine(nearTopRight, nearBottomRight);
            //Gizmos.DrawLine(nearBottomRight, nearBottomLeft);
            //Gizmos.DrawLine(nearBottomLeft, nearTopLeft);

            //Gizmos.DrawLine(farTopLeft, farTopRight);
            //Gizmos.DrawLine(farTopRight, farBottomRight);
            //Gizmos.DrawLine(farBottomRight, farBottomLeft);
            //Gizmos.DrawLine(farBottomLeft, farTopLeft);
            //cameraPosition = transform.worldToLocalMatrix.MultiplyPoint(cameraPosition);
            Gizmos.DrawLine(cameraPosition, farTopLeft);
            Gizmos.DrawLine(cameraPosition, farTopRight);
            Gizmos.DrawLine(cameraPosition, farBottomRight);
            Gizmos.DrawLine(cameraPosition, farBottomLeft);

            if (castRays)
            {
                CastRaysFromFrustum(cameraPosition, farTopLeft, farTopRight, farBottomLeft, farBottomRight);
            }

            Gizmos.matrix = matrix;
        }
    }

    private void CastRaysFromFrustum(Vector3 cameraPosition, params Vector3[] corners)
    {


        foreach (Vector3 corner in corners)
        {
            //Vector3 worldPos = cameraComponent.transform.localToWorldMatrix.MultiplyPoint(corner);
            Vector3 worldPos = corner;
            //cameraPosition.transform.localToWorldMatrix.MultiplyPoint(cameraPosition);



            Vector3 direction = worldPos - cameraPosition;

            if (Physics.Raycast(cameraPosition, direction.normalized, out RaycastHit hit, rayLength, raycastLayerMask))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(cameraPosition, hit.point);
                Debug.DrawLine(cameraPosition, hit.point, Color.red);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(cameraPosition, cameraPosition + direction.normalized * rayLength);
                Debug.DrawLine(cameraPosition, cameraPosition + direction.normalized * rayLength, Color.blue);
            }
        }
    }
}