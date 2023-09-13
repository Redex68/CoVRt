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

    [SerializeField] GameObject teleportationBall, playerInstance, tpBallTarget;
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

    void OnUseBall(InputAction.CallbackContext context)
    {
        if (teleportationBallInstance != null)
        {
            Vector3? point = tpBallScript.ActivateBall();
            if (point != null)
            {
                playerInstance.transform.position = (Vector3)point;
                Destroy(teleportationBallInstance);
            }
        }
        else
        {
            teleportationBallInstance = Instantiate(teleportationBall);
            teleportationBallInstance.transform.position = tpBallTarget.transform.position;
            tpBallScript = teleportationBallInstance.GetComponent<TeleportationBall>();
            tpBallScript.target = tpBallTarget.transform;
        }
    }

    void OnRecall(InputAction.CallbackContext context)
    {
        if (teleportationBallInstance != null)
        {
            Destroy(teleportationBallInstance);
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
