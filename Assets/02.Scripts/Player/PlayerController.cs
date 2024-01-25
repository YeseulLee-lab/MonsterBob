using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private VariableJoystick joystick;

    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator animator;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        joystick = UIManager.Instance.joystick;
    }

    private void Update()
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

    private void Attack()
    {

    }
}
