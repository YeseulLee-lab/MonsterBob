using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagCanvas : MonoBehaviour
{
    [SerializeField] private Button closeButton;

    [Header("Slots")]
    [SerializeField] private FieldSlot[] slots;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

    [Header("Details")]
    [SerializeField] private GameObject detailsObject;
    [SerializeField] private GameObject invenEmptyObject;
    [SerializeField] private Text lootName;
    [SerializeField] private Image lootImage;
    [SerializeField] private Text description;

    private void OnEnable()
    {
        transform.DOScale(1f, 0.5f).SetEase(Ease.InExpo);

        if (InventoryManager.Instance.lootInvenDic.Count == 0)
        {
            detailsObject.SetActive(false);
            invenEmptyObject.SetActive(true);
            nextButton.interactable = false;
            prevButton.interactable = false;
        }
        else
        {
            SetData();
            detailsObject.SetActive(true);
            invenEmptyObject.SetActive(false);
            lootName.text = slots[0].loot.name;
            description.text = slots[0].loot.desc;
            lootImage.sprite = slots[0].loot.sprite;
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
            transform.DOScale(0f, 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        });
    }

    private void SetData()
    {
        int i = 0;
        foreach (KeyValuePair<Loot, int> pair in InventoryManager.Instance.lootInvenDic)
        {
            slots[i].loot = pair.Key;
            slots[i].itemImage.gameObject.SetActive(true);
            slots[i].itemImage.sprite = pair.Key.sprite;
            slots[i].itemCountText.text = pair.Value.ToString();
            i++;
        }
    }

    public void SetDetailData(string name, string desc, Sprite sprite)
    {
        lootName.text = name;
        description.text = desc;
        lootImage.sprite = sprite;
    }
}
