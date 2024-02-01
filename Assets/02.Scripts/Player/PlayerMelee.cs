using DG.Tweening;
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

    private bool isHurting;

    [Header("------HealthInfo------")]
    [SerializeField] private float playerMaxHealth;
    [SerializeField] private float playerCurHealth;

    private HUDCanvas hudCanvas;

    [SerializeField] private LayerMask enemyLayers;

    private void Start()
    {
        hudCanvas =  UIManager.Instance.canvases[1].GetComponent<HUDCanvas>();

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
        FieldManager.Instance.playerState = FieldManager.PlayerState.Dead;
    }

    public void GetDamaged(float damage, Vector3 enemyDir, float strength)
    {
        if (FieldManager.Instance.playerState == FieldManager.PlayerState.Dead || isHurting)
        {
            return;
        }

        GetComponentInChildren<Animator>().SetTrigger("isDamaged");
        FieldManager.Instance.playerState = FieldManager.PlayerState.Damage;

        KnockBack(enemyDir, strength);
        if (playerCurHealth <= 0)
        {
            return;
        }

        playerCurHealth -= damage;
        hudCanvas.HealthBarUpdate(playerCurHealth);

        StartCoroutine(CoHurt());

        if (playerCurHealth <= 0)
        {
            Dead();
        }
    }

    private IEnumerator CoHurt()
    {
        isHurting = true;
        GetComponentInChildren<SpriteRenderer>().DOFade(0.4f, 0.2f);
        yield return new WaitForSeconds(2f);
        GetComponentInChildren<SpriteRenderer>().DOFade(1f, 0.2f);
        isHurting = false;
    }

    private void KnockBack(Vector3 enemyDir, float strength)
    {
        Vector3 direction = (transform.position - enemyDir).normalized;
        GetComponent<Rigidbody>().AddForce(direction * strength, ForceMode.Impulse);
    }
}
