using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookCanvas : MonoBehaviour
{
    [SerializeField] private Button closeButton;

    [Header("Slots")]
    [SerializeField] private FieldSlot[] slots;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

    private void OnEnable()
    {
        transform.DOScale(1f, 0.5f).SetEase(Ease.InExpo);

        if (InventoryManager.Instance.lootInvenDic.Count != 0)
        {
            SetData();
            if (InventoryManager.Instance.lootInvenDic.Count > slots.Length)
            {
                nextButton.interactable = true;
                prevButton.interactable = false;
            }
            else
            {
                nextButton.interactable = false;
                prevButton.interactable = false;
            }
        }
    }

    private void Start()
    {
        closeButton.onClick.AddListener(delegate
        {
            AudioManager.instance.PlaySound("UIClose");
            transform.DOScale(0f, 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        });
    }

    private void SetData()
    {
        int i = 0;
        foreach (KeyValuePair<LootData, int> pair in InventoryManager.Instance.lootInvenDic)
        {
            slots[i].loot = pair.Key;
            slots[i].itemImage.gameObject.SetActive(true);
            slots[i].itemImage.sprite = pair.Key.sprite;
            slots[i].itemCountText.text = pair.Value.ToString();
            i++;
        }
    }
}
