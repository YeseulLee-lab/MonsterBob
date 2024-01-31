using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldInventory : MonoBehaviour
{
    [SerializeField] private FieldSlot[] fieldSlots;
    [SerializeField] private int maxSlotCount;

    private void Start()
    {
        InventoryManager.Instance.fieldInventory = this;
    }

    public void GetItem(Loot loot)
    {
        foreach (FieldSlot slot in fieldSlots)
        {
            if (slot.itemImage.sprite == null)
            {
                slot.loot = loot;
                slot.itemImage.gameObject.SetActive(true);
                slot.itemImage.sprite = loot.sprite;
                slot.itemCount ++;
                slot.itemCountText.text = slot.itemCount.ToString();
                break;
            }
            else
            {
                if (slot.itemImage.sprite.name.Equals(loot.sprite.name))
                {
                    slot.itemCount ++;
                    slot.itemCountText.text = slot.itemCount.ToString();
                    break;
                }
            }
        }
    }
}
