using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator animator;
    private Rigidbody rb;

    [Header("Player Control")]
    private VariableJoystick joystick;

    #region Unity Life Cycle
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        joystick = JoyStickManager.Instance.joystick;

        JoyStickManager.Instance.state = JoyStickManager.PlayerState.Idle;
    }

    private void Update()
    {
        switch (JoyStickManager.Instance.state)
        {
            case JoyStickManager.PlayerState.Walk:
                if (joystick != null)
                    Move();
                break;
            case JoyStickManager.PlayerState.AttackSword:
                Attack();
                break;
        }
    }
    #endregion

    private void Attack()
    {
        animator.SetTrigger("isAttacking");
        JoyStickManager.Instance.state = JoyStickManager.PlayerState.Idle;
    }

    private void Move()
    {
        float x = joystick.Horizontal;
        float y = joystick.Vertical;

        Vector3 moveDir = new Vector3(x, 0, y);
        rb.velocity = moveDir * speed;

        animator.SetFloat("deltaX", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("deltaY", Mathf.Abs(rb.velocity.z));

        if (x != 0 && x < 0)
        {
            sr.flipX = true;
        }
        else if (x != 0 && x > 0)
        {
            sr.flipX = false;
        }
    }
}
