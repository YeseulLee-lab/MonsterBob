using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterPatrol : MonoBehaviour
{
    [Header("Health Canvas")]
    [SerializeField] private GameObject healthCanvas;
    [SerializeField] private Text healthText;
    [SerializeField] private Image healthBar;
    [SerializeField] private Text monsterName;

    [Header("Monster Info")]
    public Monster monster;
    [SerializeField] private float curHealth;
    [SerializeField] private float strength;

    [Header("Patrol")]
    [SerializeField] private float speed;
    private Vector3 moveSpot;
    [SerializeField] private float startWaitTime;
    private float waitTime;
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;


    [Header("Attack")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float followRange;
    [SerializeField] private float attackRange;
    [SerializeField] private GameObject damageEffect;
    [SerializeField] private float playerDamage;
    private string[] clickAttackSounds = new string[3]{ "CharacterAttack", "CharacterAttack2", "CharacterAttack3" };

    [Header("--------------------")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sr;
    private Rigidbody rigid;
    private PlayerController player;
    private Vector3 originScale;

    private bool isHurting = false;

    private void Start()
    {
        waitTime = startWaitTime;

        moveSpot = new Vector3(UnityEngine.Random.Range(minX, maxX), transform.position.y, UnityEngine.Random.Range(minZ, maxZ));
        if (transform.position.x - moveSpot.x > 0)
        {
            sr.flipX = false;
        }
        animator.SetBool("isWalking", true);

        rigid = GetComponent<Rigidbody>();
        monsterName.text = monster.monsterName;
        curHealth = monster.maxHealth;
        player = FindObjectOfType<PlayerController>();
        originScale = transform.localScale;

        healthText.text = string.Format("{0} / {1}", curHealth, monster.maxHealth);
    }

    private void Update()
    {
        if (Dead() || isHurting)
        {
            return;
        }

        if (!FieldManager.Instance.isClickerMode)
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
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Camera.main.transform.position == FieldManager.Instance.MonsterVirtualCam.transform.position)
                {
                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(transform.DOScale(originScale * 1.1f, 0.1f)).Append(transform.DOScale(originScale * 0.9f, 0.1f)).Append(transform.DOScale(originScale * 1f, 0.1f));
                    GetClickerDamaged(playerDamage * 0.2f);

                    Debug.Log("연타!!");
                }
            }
        }
    }

    private void GetClickerDamaged(float damage)
    {
        damageEffect.GetComponent<DamageEffectPopup>().damage = damage;
        Instantiate(damageEffect, transform.position, Quaternion.identity);

        AudioManager.instance.PlaySound(clickAttackSounds[UnityEngine.Random.Range(0, 3)]);

        curHealth -= damage;
        Debug.Log(curHealth);

        if (Dead())
        {
            FieldManager.Instance.SwitchCamera(false, transform);
            DropLoots();
            gameObject.SetActive(false);
            AudioManager.instance.PlaySound("MonsterDeath2");
            Destroy(gameObject, 1f);
        }

        healthBar.fillAmount = curHealth / monster.maxHealth;
        healthText.text = string.Format("{0} / {1}", curHealth, monster.maxHealth);
    }

    public void GetDamaged(float damage, Vector3 playerDir, float strength)
    {
        StartCoroutine(CoHurt());
        animator.SetTrigger("isDamaged");
        damageEffect.GetComponent<DamageEffectPopup>().damage = damage;
        Instantiate(damageEffect, transform.position, Quaternion.identity);
        KnockBack(playerDir, strength);
        playerDamage = damage;

        if (curHealth <= 0)
        {
            return;
        }

        FieldManager.Instance.SwitchCamera(true, transform);

        curHealth -= damage;

        if (Dead())
        {
            FieldManager.Instance.SwitchCamera(false, transform);
            DropLoots();
            gameObject.SetActive(false);
            AudioManager.instance.PlaySound("MonsterDeath2");
            Destroy(gameObject, 1f);
        }

        healthBar.fillAmount = curHealth / monster.maxHealth;
        healthText.text = string.Format("{0} / {1}", curHealth, monster.maxHealth);
    }

    private IEnumerator CoHurt()
    {
        //스턴
        isHurting = true;
        yield return new WaitForSeconds(1f);
        isHurting = false;
    }

    private bool Dead()
    {
        if (curHealth <= 0)
        {
            animator.SetTrigger("isDead");
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DropLoots()
    {
        int randomNumber = UnityEngine.Random.Range(1, 101);

        foreach (LootObject loot in monster.loots)
        {
            if (randomNumber <= loot.loot.dropChance)
            {
                int count = 0;
                if (loot.loot.dropChance >= 50)
                {
                    count = UnityEngine.Random.Range(1, 4);
                }

                if (loot.loot.dropChance < 50)
                {
                    count = UnityEngine.Random.Range(0, 3);
                }
                Debug.Log("Loot 확률" + randomNumber + " 개수 :" + count);
                loot.GetComponent<LootObject>().player = player.gameObject;
                
                for (int i = 0; i < count; i++)
                {
                    Instantiate(loot.gameObject, transform.position, Quaternion.identity);
                }
            }
        }
    }

    private void KnockBack(Vector3 playerDir, float strength)
    {
        Vector3 direction = (transform.position - playerDir).normalized;
        rigid.AddForce(direction * strength, ForceMode.Impulse);
    }

    private void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveSpot, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, moveSpot) < 0.2f)
        {
            if (waitTime <= 0)
            {
                animator.SetBool("isWalking", true);
                moveSpot = new Vector3(UnityEngine.Random.Range(minX, maxX), transform.position.y, UnityEngine.Random.Range(minZ, maxZ));
                waitTime = startWaitTime;

                if (transform.position.x - moveSpot.x > 0)
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
        moveSpot = new Vector3(UnityEngine.Random.Range(minX, maxX), transform.position.y, UnityEngine.Random.Range(minZ, maxZ));
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
    public string monsterName;
    public Sprite sprite;
    public float maxHealth;
    public float damage;
    public LootObject[] loots;
    public MapCanvas.LandType landType;
}