using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterPatrol : MonoBehaviour
{
    [Header("Health Canvas")]
    [SerializeField] private Text healthText;
    [SerializeField] private Image healthBar;
    [SerializeField] private Text monsterName;

    [Header("Monster Info")]
    public Monster monster;
    [SerializeField] private float curHealth;
    [SerializeField] private float strength;

    [Header("Patrol")]
    [SerializeField] private float speed;
    [SerializeField] private Transform moveSpot;
    [SerializeField] private float startWaitTime;
    private float waitTime;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minZ;
    [SerializeField] private float maxZ;


    [Header("Attack")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float followRange;
    [SerializeField] private float attackRange;

    [Header("--------------------")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sr;
    private Rigidbody rigid;
    private PlayerController player;

    public float Strength
    {
        get
        {
            return strength;
        }
    }

    private void Start()
    {
        waitTime = startWaitTime;

        moveSpot.position = new Vector3(UnityEngine.Random.Range(minX, maxX), transform.position.y, UnityEngine.Random.Range(minZ, maxZ));
        if (transform.position.x - moveSpot.position.x > 0)
        {
            sr.flipX = false;
        }
        animator.SetBool("isWalking", true);

        rigid = GetComponent<Rigidbody>();
        monsterName.text = monster.name;
        curHealth = monster.maxHealth;
        player = FindObjectOfType<PlayerController>();

        healthText.text = string.Format("{0} / {1}", curHealth, monster.maxHealth);
    }

    private void Update()
    {
        if (PlayerInSight())
        {
            Follow();
        }
        else
        {
            Patrol();
        }
    }

    public void GetDamaged(float damage, Vector3 playerDir, float strength)
    {
        animator.SetTrigger("isDamaged");
        KnockBack(playerDir, strength);

        if (curHealth <= 0)
        {
            return;
        }

        curHealth -= damage;
        if (curHealth <= 0)
        {
            Dead();
        }

        healthBar.fillAmount = curHealth / monster.maxHealth;
        healthText.text = string.Format("{0} / {1}", curHealth, monster.maxHealth);
    }

    private void Dead()
    {
        animator.SetTrigger("isDead");
        DropLoots();
        Destroy(gameObject, 1f);
    }

    private void DropLoots()
    {
        int count = monster.loots.Length;
        monster.loots[UnityEngine.Random.Range(0, count - 1)].GetComponent<LootObject>().player = player.gameObject;
        Instantiate(monster.loots[UnityEngine.Random.Range(0, count - 1)].gameObject, transform.position, Quaternion.identity);
    }

    private void KnockBack(Vector3 playerDir, float strength)
    {
        Vector3 direction = (transform.position - playerDir).normalized;
        rigid.AddForce(direction * strength, ForceMode.Impulse);
    }

    private void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveSpot.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, moveSpot.position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                animator.SetBool("isWalking", true);
                moveSpot.position = new Vector3(UnityEngine.Random.Range(minX, maxX), transform.position.y, UnityEngine.Random.Range(minZ, maxZ));
                waitTime = startWaitTime;

                if (transform.position.x - moveSpot.position.x > 0)
                {
                    sr.flipX = true;
                }
                else
                {
                    sr.flipX = false;
                }
            }
            else
            {
                waitTime -= Time.deltaTime;
                animator.SetBool("isWalking", false);
            }
        }
    }

    private void Follow()
    {
        if (Vector3.Distance(attackPoint.position, player.transform.position) <= attackRange)
        {
            Attack();
        }
        else
        {
            animator.SetBool("isWalking", true);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    private void Attack()
    {
        animator.SetTrigger("isAttacking");
        if (transform.position.x - player.transform.position.x > 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }

        player.GetComponent<PlayerMelee>().GetDamaged(monster.damage, transform.position, strength);
        moveSpot.position = new Vector3(UnityEngine.Random.Range(minX, maxX), transform.position.y, UnityEngine.Random.Range(minZ, maxZ));
    }

    private bool PlayerInSight()
    {
        if (Vector3.Distance(attackPoint.position, player.transform.position) <= followRange)
        {
            return true;
        }

        return false;
    }
}

[Serializable]
public class Monster
{
    public string name;
    public float maxHealth;
    public float damage;
    public LootObject[] loots;
}