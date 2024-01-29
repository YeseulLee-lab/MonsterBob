using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator animator;
    private Rigidbody rb;
    private bool isAttacking = false;

    public bool IsAttacking
    {
        set { isAttacking = value; }
    }

    [Header("Player Control")]
    private VariableJoystick joystick;
    private PlayerMelee playerMelee;

    #region Unity Life Cycle
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMelee = GetComponent<PlayerMelee>();
        joystick = FieldUIManager.Instance.joystick;

        FieldUIManager.Instance.state = FieldUIManager.PlayerState.Idle;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            FieldUIManager.Instance.state = FieldUIManager.PlayerState.AttackSword;
        }

        switch (FieldUIManager.Instance.state)
        {
            case FieldUIManager.PlayerState.Walk:
#if UNITY_EDITOR
                if (UnityEngine.Device.Application.platform == RuntimePlatform.Android)
                {
                    if (joystick != null)
                        Move();
                }
                else
                    Move();
#elif PLATFORM_ANDROID
                    if (joystick != null)
                        Move();
#endif
                break;
            case FieldUIManager.PlayerState.AttackSword:
                Attack();
                break;
            case FieldUIManager.PlayerState.Idle:
#if UNITY_EDITOR
                if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
                {
                    FieldUIManager.Instance.state = FieldUIManager.PlayerState.Walk;
                }
#endif
                animator.SetFloat("deltaX", Mathf.Abs(rb.velocity.x));
                animator.SetFloat("deltaY", Mathf.Abs(rb.velocity.z));
                break;
            case FieldUIManager.PlayerState.Dead:
                animator.SetTrigger("isDead");
                FieldUIManager.Instance.state = FieldUIManager.PlayerState.Idle;
                break;
            case FieldUIManager.PlayerState.Damage:
                
                FieldUIManager.Instance.state = FieldUIManager.PlayerState.Idle;
                break;
        }
    }
#endregion

    private void Attack()
    {
        isAttacking = true;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return;
        animator.SetTrigger("isAttacking");

        playerMelee.Attack();
        FieldUIManager.Instance.state = FieldUIManager.PlayerState.Idle;
    }

    private void Move()
    {
        if(isAttacking)
            return;
        float x;
        float y;
#if UNITY_EDITOR
        if (UnityEngine.Device.Application.platform == RuntimePlatform.Android)
        {
            x = joystick.Horizontal;
            y = joystick.Vertical;
        }
        else
        {
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
        }
#elif PLATFORM_ANDROID

        float x = joystick.Horizontal;
        float y = joystick.Vertical;
#endif
        if (x == 0 && y == 0)
        {
            FieldUIManager.Instance.state = FieldUIManager.PlayerState.Idle;
        }
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
