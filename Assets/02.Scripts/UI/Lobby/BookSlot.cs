using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookSlot : MonoBehaviour
{
    public Image itemImage;
    public LootData loot;

    [SerializeField] private BookCanvas bookCanvas;

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
            itemImage.gameObject.SetActive(true);
        }
        else
        {
            itemImage.sprite = null;
            itemImage.gameObject.SetActive(false);
        }
    }
}
