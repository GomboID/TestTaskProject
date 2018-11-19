using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObeliskActivationController : ActivatedObject
{
    GameObject m_Player;
    public Transform LookObject;
    private float m_DegreeCounter;

    private void Start()
    {
        m_IsCanActivateBySword = false;
        GameController.Instance.DiactivateAllChainObjects += DiactivateObject;
    }

    private void Update()
    {
        if (m_Player == null)
            DetectPlayer();
        else
            FollowPlayer();
    }

    private void DetectPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5);
        foreach (Collider collider in hitColliders)
        {
            if (collider.GetComponent<PlayerController>())
            {
                m_Player = collider.gameObject;
                LookObject.LookAt(m_Player.transform);
                m_DegreeCounter = 0f;
            }
        }
    }

    private void FollowPlayer()
    {
        if (m_IsActivated || m_IsCheckActivationStarted)
        {
            return;
        }
        float distance = Vector3.Distance(m_Player.transform.position, transform.position);
        if (distance > 5.5f)
        {
            m_Player = null;
            return;
        }
        CalculateDegree();
        if (Mathf.Abs(m_DegreeCounter) > 360)
        {
            m_IsCheckActivationStarted = true;
            ActivateObject();
        }
    }

    private void CalculateDegree()
    {
        var previewForward = LookObject.forward;
        LookObject.LookAt(m_Player.transform);
        var currentForward = LookObject.forward;
        var ang = Vector3.Angle(currentForward, previewForward);
        if (ang > 0.001)
        {
            if (Vector3.Cross(currentForward, previewForward).y < 0) ang = -ang;
            m_DegreeCounter += ang;
        }
    }

    protected override void ActivationFail()
    {
        m_IsActivated = false;
        m_DegreeCounter = 0;
        m_IsCheckActivationStarted = false;
        ActivationEffect = Instantiate(FailActivationEffect, transform);
    }

    protected override void DiactivateObject()
    {
        m_IsActivated = false;
        m_DegreeCounter = 0;
        m_IsCheckActivationStarted = false;
        Destroy(ActivationEffect);
    }
}
