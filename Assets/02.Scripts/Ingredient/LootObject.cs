using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootObject : MonoBehaviour
{
    public Loot loot;

    private float followSpeed = 0.35f;
    private float originScale;
    private bool isDrop = false;

    public GameObject player;

    private void Start()
    {
        loot.sprite = GetComponent<SpriteRenderer>().sprite;
        originScale = transform.localScale.x;
        transform.localScale = Vector3.zero;
        transform.DOScale(originScale, 0.3f).SetEase(Ease.InBounce).OnComplete(() =>
        {
            isDrop = true;
        });
    }

    private void Update()
    {
        if (isDrop)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < 0.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, followSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            InventoryManager.Instance.GetLoots(loot);
            Destroy(gameObject);
        }
    }
}

[Serializable]
public class Loot
{
    public Sprite sprite;
    public string name;
    public string desc;
    public int uid;
}