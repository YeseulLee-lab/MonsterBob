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

    public float TankDamage
    {
        get
        {
            return monster.tankDamage;
        }
    }

    public float Strength
    {
        get
        {
            return strength;
        }
    }

    [Header("--------------------")]
    [SerializeField] private Animator animator;
    private Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        monsterName.text = monster.name;
        curHealth = monster.maxHealth;

        healthText.text = string.Format("{0} / {1}", curHealth, monster.maxHealth);
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
        Destroy(gameObject, 1f);
    }

    private void KnockBack(Vector3 playerDir, float strength)
    {
        Vector3 direction = (transform.position - playerDir).normalized;
        rigid.AddForce(direction * strength, ForceMode.Impulse);
    }
}

[Serializable]
public class Monster
{
    public string name;
    public float maxHealth;
    public float tankDamage;
    public float skillDamage;
    public Loot[] loots;
}

[Serializable]
public class Loot
{
    public Sprite sprite;
    public string lootName;
}