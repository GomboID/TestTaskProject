using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareActivationController : ActivatedObject
{
    private bool m_IsCheckDisabled = false;
    private void Start()
    {
        m_IsCanActivateBySword = false;
        GameController.Instance.DiactivateAllChainObjects += DiactivateObject;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (m_IsActivated || m_IsCheckActivationStarted)
            return;       
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            if (collision.gameObject.GetComponent<Rigidbody>().velocity == Vector3.zero)
            {
                m_IsCheckActivationStarted = true;
                if (GameController.Instance.ActivateObject(gameObject))
                    ActivationSuccess();
                else
                    ActivationFail();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            m_IsCheckActivationStarted = false;
        }
    }
}
