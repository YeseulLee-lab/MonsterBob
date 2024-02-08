using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CookCanvas : MonoBehaviour
{
    [SerializeField] private Button closeButton;

    [Header("Slots")]
    [SerializeField] private FieldSlot[] slots;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;
    public Text slotText;

    [Header("Cooks")]
    public CookData[] cookDatas;

    public LootData selectedLoot;
    public UnityAction OnCooker;

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

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Refresh = () =>
            {
                SetData();
            };
        }
    }

    public void SetData()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].loot = null;
            slots[i].itemImage.gameObject.SetActive(false);
            slots[i].itemImage.sprite = null;
            slots[i].itemCountText.text = string.Empty;
            slots[i].itemCount = 0;
        }

        if (InventoryManager.Instance.lootInvenDic.Count > 0)
        {
            int i = 0;
            foreach (KeyValuePair<LootData, int> pair in InventoryManager.Instance.lootInvenDic)
            {
                slots[i].loot = pair.Key;
                slots[i].itemImage.gameObject.SetActive(true);
                slots[i].itemImage.sprite = pair.Key.sprite;
                slots[i].itemCountText.text = pair.Value.ToString();
                slots[i].itemCount = pair.Value;
                i++;
            }
        }
    }
}
