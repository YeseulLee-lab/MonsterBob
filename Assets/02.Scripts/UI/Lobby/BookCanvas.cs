using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookCanvas : MonoBehaviour
{
    [SerializeField] private Button closeButton;

    [Header("BookSlots")]
    [SerializeField] private BookSlot[] bookSlots;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

    [Header("Details")]
    [SerializeField] private GameObject detailsObject;
    [SerializeField] private Button background;
    [SerializeField] private Text lootName;
    [SerializeField] private Image lootImage;
    [SerializeField] private Text description;

    private void OnEnable()
    {
        transform.DOScale(1f, 0.5f).SetEase(Ease.InExpo);

        /*if (InventoryManager.Instance.lootInvenDic.Count == 0)
        {
            detailsObject.SetActive(false);
            nextButton.interactable = false;
            prevButton.interactable = false;
        }
        else
        {
            SetData();
            detailsObject.SetActive(true);
            lootName.text = slots[0].loot.lootName;
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
        }*/

        
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

        background.onClick.AddListener(delegate
        {
            detailsObject.transform.DOScale(0f, 0.3f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                detailsObject.SetActive(false);
            });
        });

        detailsObject.transform.localScale = Vector3.zero;
    }

    private void SetData()
    {
        int i = 0;
        foreach (KeyValuePair<LootData, int> pair in InventoryManager.Instance.lootInvenDic)
        {
            bookSlots[i].loot = pair.Key;
            bookSlots[i].itemImage.gameObject.SetActive(true);
            bookSlots[i].itemImage.sprite = pair.Key.sprite;
            i++;
        }
    }

    public void SetDetailData(string name, string desc, Sprite sprite)
    {
        detailsObject.SetActive(true);
        detailsObject.transform.DOScale(1f, 0.3f).SetEase(Ease.OutExpo);
        lootName.text = name;
        description.text = desc;
        lootImage.sprite = sprite;
    }
}
