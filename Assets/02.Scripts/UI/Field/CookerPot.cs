using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CookerPot : MonoBehaviour, IDropHandler
{
    [SerializeField] private CookCanvas cookCanvas;
    [SerializeField] private Image cookerImage;
    [SerializeField] private Animator cookerAnimator;
    [SerializeField] private Image clock;

    [Header("Cooker Data")]
    [SerializeField] private List<LootData> lootList;
    [SerializeField] private Image[] lootImageArr = new Image[6];

    [Header("Buttons")]
    [SerializeField] private Button cookButton;
    [SerializeField] private Button resetButton;

    private void Start()
    {
        cookButton.onClick.AddListener(OnClickCookButton);

        resetButton.onClick.AddListener(ResetCooker);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(cookCanvas.selectedLoot == null)
            return;
        
        AudioManager.instance.PlaySound("LowPop");

        lootList.Add(cookCanvas.selectedLoot);
        if (lootList.Count > 0)
        {
            cookButton.interactable = true;
        }

        SetLootImage();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(cookerImage.transform.DOScale(1.1f, 0.3f)).Append(cookerImage.transform.DOScale(1f, 0.3f));

        cookCanvas.OnCooker();
    }

    private void SetLootImage()
    {
        if (lootList.Count > 0 || lootList.Count < lootImageArr.Length)
        {
            lootImageArr[lootList.Count - 1].enabled = true;
            lootImageArr[lootList.Count - 1].sprite = lootList[lootList.Count - 1].sprite;
        }
    }

    private void OnClickCookButton()
    {
        cookerAnimator.enabled = true;
        clock.gameObject.SetActive(true);

        cookButton.interactable = false;
        resetButton.onClick.RemoveAllListeners();
        resetButton.onClick.AddListener(ThrowAway);
        resetButton.GetComponentInChildren<Text>().text = "버리기";
    }

    private void ResetCooker()
    {
        for (int i = 0; i < lootList.Count; i++)
        {
            InventoryManager.Instance.GetLoots(lootList[i]);
        }

        lootList.Clear();
        cookButton.interactable = false;

        for (int i = 0; i < lootImageArr.Length; i++)
        {
            lootImageArr[i].enabled = false;
            lootImageArr[i].sprite = null;
        }

        cookCanvas.SetData();        
    }

    private void ThrowAway()
    {
        lootList.Clear();
        cookButton.interactable = false;

        for (int i = 0; i < lootImageArr.Length; i++)
        {
            lootImageArr[i].enabled = false;
            lootImageArr[i].sprite = null;
        }

        resetButton.onClick.RemoveAllListeners();
        resetButton.onClick.AddListener(ThrowAway);
        resetButton.GetComponentInChildren<Text>().text = "초기화";

        clock.gameObject.SetActive(false);

        cookerAnimator.enabled = false;
    }
}
