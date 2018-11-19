using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance = null;
    enum Controls
    {
        moveForward,
        moveLeft,
        moveRight,
        moveBack,
        jump,
        lightAttack,
        strongAttack,
        stopBackAndForthMove,
        stopLeftAndRightMove
    }


    public Camera PlayerCamera;
    [HideInInspector]
    public bool IsAttackStarted = false;

    private Animator m_AnimatorController;

    private float m_MovementDirection = 0;
    private float m_MovementSpeed = 0f;

    private float m_SpeedHorizontal = 2f;
    private float m_SpeedVertical = 2f;
    private float m_Yaw = 0f;
    private float m_Pitch = 0f;
    private bool m_IsStopMoving = false;
    


    void Start ()
    {
        if (Instance == null)
            Instance = this;
        m_AnimatorController = GetComponent<Animator>();
        m_AnimatorController.SetFloat("Speed", m_MovementSpeed);
        m_AnimatorController.SetFloat("Direction", m_MovementDirection);
	}

    void Update()
    {
        MovementPlayer();
        RotationPlayer();
    }

    private void MovementPlayer()
    {
#if UNITY_STANDALONE
        if (Input.GetKey(KeyCode.W))
        {
            Movement(Controls.moveForward, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Movement(Controls.moveBack, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Movement(Controls.moveLeft, -1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Movement(Controls.moveRight, 1);
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            Movement(Controls.stopBackAndForthMove);
        }

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            Movement(Controls.stopLeftAndRightMove);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Movement(Controls.jump);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Movement(Controls.lightAttack);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Movement(Controls.strongAttack);
        }

#endif
#if UNITY_ANDROID
#endif
        m_AnimatorController.SetFloat("Speed", m_MovementSpeed);
        m_AnimatorController.SetFloat("Direction", m_MovementDirection);
    }

    private void Movement(Controls _control, float _value = 0f)
    {
        switch (_control)
        {
            case Controls.moveForward:
                m_MovementSpeed = Mathf.Lerp(m_MovementSpeed, _value, 0.05f);
                break;
            case Controls.moveBack:
                m_MovementSpeed = Mathf.Lerp(m_MovementSpeed, _value, 0.05f);
                break;
            case Controls.moveLeft:
                m_MovementDirection = Mathf.Lerp(m_MovementDirection, _value, 0.05f);
                break;
            case Controls.moveRight:
                m_MovementDirection = Mathf.Lerp(m_MovementDirection, _value, 0.05f);
                break;
            case Controls.jump:
                m_AnimatorController.SetTrigger("Jump");
                break;
            case Controls.lightAttack:
                int id = Random.Range(1, 3);
                m_AnimatorController.SetInteger("LightAttackId", id);
                m_AnimatorController.SetTrigger("LightAttack");
                break;
            case Controls.strongAttack:
                m_AnimatorController.SetTrigger("StrongAttack");
                break;
            case Controls.stopBackAndForthMove:
                m_MovementSpeed = Mathf.Lerp(m_MovementSpeed, _value, 0.25f);
                break;
            case Controls.stopLeftAndRightMove:
                m_MovementDirection = Mathf.Lerp(m_MovementDirection, 0, 0.25f);
                break;
        }
    }

    private void RotationPlayer()
    {
        m_Yaw += m_SpeedHorizontal *  Input.GetAxis("Mouse X");
        m_Pitch -= m_SpeedVertical * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(0f, m_Yaw, 0f);
        PlayerCamera.transform.localEulerAngles = new Vector3(m_Pitch, 0f, 0f);
    }

    private void AttackAnimationStarted()
    {
        IsAttackStarted = true;
    }

    private void AttackAnimationEnded()
    {
        IsAttackStarted = false;
    }
}
