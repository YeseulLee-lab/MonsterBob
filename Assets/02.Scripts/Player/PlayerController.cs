using DG.Tweening;
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
    private bool isDead = false;

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
        joystick = UIManager.Instance.canvases[0].GetComponent<MainFieldCanvas>().joystick;

        FieldManager.Instance.playerState = FieldManager.PlayerState.Idle;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            FieldManager.Instance.playerState = FieldManager.PlayerState.AttackSword;
        }

        switch (FieldManager.Instance.playerState)
        {
            case FieldManager.PlayerState.Walk:
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
            case FieldManager.PlayerState.AttackSword:
                Attack();
                break;
            case FieldManager.PlayerState.Idle:
#if UNITY_EDITOR
                if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
                {
                    FieldManager.Instance.playerState = FieldManager.PlayerState.Walk;
                }
#endif
                animator.SetFloat("deltaX", Mathf.Abs(rb.velocity.x));
                animator.SetFloat("deltaY", Mathf.Abs(rb.velocity.z));
                break;
            case FieldManager.PlayerState.Dead:
                animator.SetBool("isDead", true);
                //animator.enabled = false;
                isDead = true;
                break;
            case FieldManager.PlayerState.Damage:
                FieldManager.Instance.playerState = FieldManager.PlayerState.Idle;
                break;
        }
    }
#endregion

    private void Attack()
    {
        isAttacking = true;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return;
        AudioManager.instance.PlaySound("CharacterAttack0");
        animator.SetTrigger("isAttacking");

        playerMelee.Attack();
        FieldManager.Instance.playerState = FieldManager.PlayerState.Idle;
    }

    private void Move()
    {
        if(isAttacking && isDead)
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

        x = joystick.Horizontal;
        y = joystick.Vertical;
#endif
        if (x == 0 && y == 0)
        {
            FieldManager.Instance.playerState = FieldManager.PlayerState.Idle;
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
