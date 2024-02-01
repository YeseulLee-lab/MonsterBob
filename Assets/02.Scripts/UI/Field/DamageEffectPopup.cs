using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffectPopup : MonoBehaviour
{
    public float damage;
    [SerializeField] private Text damageText;

    private Rigidbody rb;
    
    [SerializeField] private float initialYVelocity = 1f;
    [SerializeField] private float initialXVelocityRange = 0.5f;
    [SerializeField] private float lifeTime = 0.4f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        damageText = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        damageText.text = damage.ToString();
        rb.velocity =
            new Vector2(Random.Range(-initialXVelocityRange, initialXVelocityRange), initialYVelocity);
        Destroy(gameObject, lifeTime);
    }
}
