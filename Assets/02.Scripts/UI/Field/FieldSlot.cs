using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FieldSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Field Slot Data")]
    public Image itemImage;
    public Text itemCountText;
    public int itemCount;
    public LootData loot;
    public GameObject lootNameObject;
    public Text lootNameText;

    [Header("Item Drag")]
    [SerializeField] private Transform itemDragParent;
    private Vector3 originItemPos;
    private Vector2 originItemScale;

    [Header("Send Data")]
    [SerializeField] private CookCanvas cookCanvas;
    public UnityAction Refresh;

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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(SceneManager.GetActiveScene().name != "02.Lobby")
            return;
        if (loot != null)
        {
            cookCanvas.selectedLoot = loot;

            cookCanvas.OnCooker = () =>
            {
                DropOnCooker();
            };

            itemImage.raycastTarget = false;
            originItemScale = itemImage.GetComponent<RectTransform>().sizeDelta;
            originItemPos = itemImage.transform.position;
            itemImage.transform.SetParent(itemDragParent);
            itemImage.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (SceneManager.GetActiveScene().name != "02.Lobby")
            return;
        if (loot != null)
            itemImage.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (SceneManager.GetActiveScene().name != "02.Lobby")
            return;
        itemImage.GetComponent<RectTransform>().sizeDelta = originItemScale;
        itemImage.transform.position = originItemPos;
        itemImage.transform.SetParent(transform);
        itemImage.transform.SetAsFirstSibling();
        cookCanvas.selectedLoot = null;
        Refresh();
    }

    public void DropOnCooker()
    {
        if (itemCount > 1)
        {
            itemCount--;
            InventoryManager.Instance.UseLoots(loot);
        }
        else if (itemCount <= 1)
        {
            InventoryManager.Instance.RemoveLoots(loot);
        }
        itemImage.raycastTarget = true;
    }
}
