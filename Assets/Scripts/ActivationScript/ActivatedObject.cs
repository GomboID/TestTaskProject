using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedObject : MonoBehaviour
{
    public GameObject SuccessActivationEffect;
    public GameObject FailActivationEffect;
    [HideInInspector]
    public bool m_IsCanActivateBySword = true;

    protected GameObject ActivationEffect = null;
    protected bool m_IsActivated = false;
    protected bool m_IsCheckActivationStarted = false;
   
    void Start()
    {
        GameController.Instance.DiactivateAllChainObjects += DiactivateObject;
    }
    public void ActivateObject()
    {
        if (GameController.Instance.ActivateObject(gameObject))
            ActivationSuccess();
        else
            ActivationFail();
    }

    protected virtual void DiactivateObject()
    {
        m_IsActivated = false;
        Destroy(ActivationEffect);
    }

    protected virtual void ActivationSuccess()
    {
        m_IsActivated = true;
        m_IsCheckActivationStarted = false;
        ActivationEffect = Instantiate(SuccessActivationEffect, transform);
    }

    protected virtual void ActivationFail()
    {
        m_IsActivated = false;
        ActivationEffect = Instantiate(FailActivationEffect, transform);
    }
}
