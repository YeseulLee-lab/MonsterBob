using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookSlot : MonoBehaviour
{
    public Image itemImage;
    public LootData loot;

    [SerializeField] private BookCanvas bookCanvas;
    [SerializeField] private Image noDataImage;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate
        {
            if (loot != null)
            {
                bookCanvas.SetDetailData(loot.lootName, loot.desc, loot.sprite);
            }            
        });
    }

    private void OnEnable()
    {
        SetLootData();
    }

    private void SetLootData()
    {
        if (loot != null)
        {
            itemImage.sprite = loot.sprite;
            noDataImage.gameObject.SetActive(false);
            itemImage.gameObject.SetActive(true);
        }
        else
        {
            itemImage.sprite = null;
            noDataImage.gameObject.SetActive(true);
            itemImage.gameObject.SetActive(false);
        }
    }
}
