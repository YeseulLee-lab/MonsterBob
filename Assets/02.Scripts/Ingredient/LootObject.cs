using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootObject : MonoBehaviour
{
    public LootData loot;

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
            GameObject lootObject = new GameObject("lootObject");
            lootObject.transform.SetParent(UIManager.Instance.canvases[0].transform);

            lootObject.AddComponent<Image>().sprite = loot.sprite;

            Vector2 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
            Vector2 WorldObject_ScreenPosition = 
                new Vector2(((viewportPosition.x * UIManager.Instance.canvases[0].GetComponent<RectTransform>().sizeDelta.x) - (UIManager.Instance.canvases[0].GetComponent<RectTransform>().sizeDelta.x * 0.5f)),
                            ((viewportPosition.y * UIManager.Instance.canvases[0].GetComponent<RectTransform>().sizeDelta.y) - (UIManager.Instance.canvases[0].GetComponent<RectTransform>().sizeDelta.y * 0.5f)));

            lootObject.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;
            lootObject.GetComponent<RectTransform>().sizeDelta = new Vector2(70f, 70f);
            lootObject.transform.SetParent((UIManager.Instance.canvases[0].GetComponent<MainFieldCanvas>().backPackImage.transform));

            Sequence sequence = DOTween.Sequence();
            sequence.Append(lootObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 0f), 1f).OnComplete(() =>
                    {
                        AudioManager.instance.PlaySound("LowPop");
                        InventoryManager.Instance.GetLoots(loot);
                        Destroy(lootObject);
                        Destroy(gameObject);
                    }))
                    .Append(UIManager.Instance.canvases[0].GetComponent<MainFieldCanvas>().backPackImage.transform.DOScale(1.2f, 0.08f))
                    .Append(UIManager.Instance.canvases[0].GetComponent<MainFieldCanvas>().backPackImage.transform.DOScale(1f, 0.08f));

            
            gameObject.SetActive(false);
        }
    }
}