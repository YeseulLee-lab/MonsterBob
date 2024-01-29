using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [Header("------Attack------")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.3f;
    [SerializeField] private float damage;
    [SerializeField] private float knockBackStrength;

    [Header("------HealthInfo------")]
    [SerializeField] private float playerMaxHealth;
    [SerializeField] private float playerCurHealth;    

    [SerializeField] private LayerMask enemyLayers;

    private void Start()
    {
        playerCurHealth = playerMaxHealth;
    }

    public void Attack()
    {
        StartCoroutine(CoAttack());
    }

    private IEnumerator CoAttack()
    {
        yield return new WaitForSeconds(0.5f);
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<MonsterPatrol>().GetDamaged(damage, transform.position, knockBackStrength);
        }
        yield return new WaitForSeconds(0.5f);
        GetComponent<PlayerController>().IsAttacking = false;
    }

    public void Dead()
    {
        FieldUIManager.Instance.state = FieldUIManager.PlayerState.Dead;
    }

    public void GetDamaged(float damage, Vector3 enemyDir, float strength)
    {
        GetComponentInChildren<Animator>().SetTrigger("isDamaged");
        FieldUIManager.Instance.state = FieldUIManager.PlayerState.Damage;

        KnockBack(enemyDir, strength);
        if (playerCurHealth <= 0)
        {
            return;
        }

        playerCurHealth -= damage;
        if (playerCurHealth <= 0)
        {
            Dead();
        }
    }

    private void KnockBack(Vector3 enemyDir, float strength)
    {
        Vector3 direction = (transform.position - enemyDir).normalized;
        GetComponent<Rigidbody>().AddForce(direction * strength, ForceMode.Impulse);
    }
}
