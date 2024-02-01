using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FieldSlot : MonoBehaviour
{
    public Image itemImage;
    public Text itemCountText;
    public int itemCount;
    public LootData loot;

    public UnityAction lobbyInvenSlotAction;
    public UnityAction fieldInvenSlotAction;

    [SerializeField] private BagCanvas bagCanvas;

    private void Start()
    {
        if (itemImage.sprite == null)
        {
            itemImage.gameObject.SetActive(false);
            itemCountText.text = string.Empty;
        }

        GetComponent<Button>().onClick.RemoveAllListeners();

        if (SceneManager.GetActiveScene().name == "02.Lobby")
        {
            GetComponent<Button>().onClick.AddListener(() => 
            {
                if(loot != null)
                    bagCanvas.SetDetailData(loot.name, loot.desc, loot.sprite);
            });
        }
        else if (SceneManager.GetActiveScene().name == "04.Field")
        {
            GetComponent<Button>().onClick.AddListener(fieldInvenSlotAction);
        }
    }
}
