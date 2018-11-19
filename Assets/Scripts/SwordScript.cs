using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (PlayerController.Instance.IsAttackStarted)
        {
            ActivatedObject activatedObj = collision.gameObject.GetComponent<ActivatedObject>();
            if (activatedObj)
            {
                if (activatedObj.m_IsCanActivateBySword)
                    activatedObj.ActivateObject();
            }
        }
    }
}
