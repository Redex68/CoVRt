using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPBallSummoner : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The reference to the action to use the teleport ball")]
    InputActionReference m_teleportBallUse;

    [SerializeField]
    [Tooltip("The reference to the action to recall the teleport ball")]
    InputActionReference m_teleportBallRecall;

    [SerializeField] GameObject teleportationBall, playerInstance, playerCamera, tpBallTarget;
    GameObject teleportationBallInstance;
    TeleportationBall tpBallScript;

    // Start is called before the first frame update
    void Start()
    {
        var teleportBallUse = m_teleportBallUse != null ? m_teleportBallUse.action : null;
        if (teleportBallUse != null)
        {
            teleportBallUse.performed += OnUseBall;
            teleportBallUse.canceled += OnThrowBall;
        }
        var teleportBallRecall = m_teleportBallRecall != null ? m_teleportBallRecall.action : null;
        if (teleportBallRecall != null)
        {
            teleportBallRecall.performed += OnRecall;
        }
    }

    void OnDestroy()
    {
        var teleportBallUse = m_teleportBallUse != null ? m_teleportBallUse.action : null;
        if (teleportBallUse != null)
        {
            teleportBallUse.performed -= OnUseBall;
            teleportBallUse.canceled -= OnThrowBall;
        }
        var teleportBallRecall = m_teleportBallRecall != null ? m_teleportBallRecall.action : null;
        if (teleportBallRecall != null)
        {
            teleportBallRecall.performed -= OnRecall;
        }
    }

    void OnUseBall(InputAction.CallbackContext context)
    {
        if (teleportationBallInstance != null)
        {
            Destroy(teleportationBallInstance);
        }
        teleportationBallInstance = Instantiate(teleportationBall);
        teleportationBallInstance.transform.position = tpBallTarget.transform.position;
        tpBallScript = teleportationBallInstance.GetComponent<TeleportationBall>();
        tpBallScript.target = tpBallTarget.transform;
    }

    void OnRecall(InputAction.CallbackContext context)
    {
        if (teleportationBallInstance != null)
        {
            Vector3? point = tpBallScript.ActivateBall();
            if (point != null)
            {
                teleportationBallInstance.transform.position = new();
                Destroy(teleportationBallInstance);
                Vector3 offset = playerInstance.transform.position - playerCamera.transform.position;
                offset.y = 0;
                playerInstance.transform.position = (Vector3)point + offset;
            }
        }
    }

    void OnThrowBall(InputAction.CallbackContext context)
    {
        if (teleportationBallInstance != null && tpBallScript.target != null)
        {
            tpBallScript.ThrowBall();
        }
    }
}
