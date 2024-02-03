using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FieldSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image itemImage;
    public Text itemCountText;
    public int itemCount;
    public LootData loot;
    public GameObject lootNameObject;
    public Text lootNameText;

    private void Start()
    {
        if (itemImage.sprite == null)
        {
            itemImage.gameObject.SetActive(false);
            itemCountText.text = string.Empty;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (loot != null)
        {
            lootNameObject.SetActive(true);

            lootNameText.text = loot.lootName;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (loot != null)
        {
            lootNameObject.SetActive(false);
        }
    }
}
