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
    private List<NeedLoot> needLootList = new List<NeedLoot>();
    [SerializeField] private Image[] lootImageArr = new Image[6];
    [SerializeField] private Image cookImage;
    [SerializeField] private Sprite cookEmptySprite;

    [Header("Buttons")]
    [SerializeField] private Button cookButton;
    [SerializeField] private Button resetButton;

    private CookData[] cookDatas;
    

    private void Start()
    {
        cookButton.onClick.AddListener(OnClickCookButton);

        resetButton.onClick.AddListener(ResetCooker);

        cookDatas = cookCanvas.cookDatas;
    }

    public void OnDrop(PointerEventData eventData)
    {
#region Drop Action
        if (cookCanvas.selectedLoot == null)
            return;
        
        AudioManager.instance.PlaySound("LowPop");

        lootList.Add(cookCanvas.selectedLoot);

        bool isContain = false;

        if (needLootList.Count > 0)
        {
            foreach (NeedLoot needLoot in needLootList)
            {
                if (needLoot.lootdata != cookCanvas.selectedLoot)
                {
                    isContain = false;
                }
                else
                {
                    isContain = true;
                    needLoot.count ++;
                    break;
                }
            }

            if (!isContain)
            {
                NeedLoot newNeedLoot = new NeedLoot();
                newNeedLoot.lootdata = cookCanvas.selectedLoot;
                newNeedLoot.count = 1;
                needLootList.Add(newNeedLoot);
            }
        }
        else
        {
            NeedLoot newNeedLoot = new NeedLoot();
            newNeedLoot.lootdata = cookCanvas.selectedLoot;
            newNeedLoot.count = 1;
            needLootList.Add(newNeedLoot);
        }

        if (lootList.Count > 0)
        {
            cookButton.interactable = true;
        }

        SetLootImage();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(cookerImage.transform.DOScale(1.1f, 0.3f)).Append(cookerImage.transform.DOScale(1f, 0.3f));

        cookCanvas.OnCooker();
        #endregion
        foreach (CookData cook in cookDatas)
        {
            if (FindCookData(cook))
            {
                //요리를 찾아서 더이상 매칭할필요가없음
                return;
            }
        }
        //모두 돌았으나 요리가 없음.
        cookImage.sprite = cookEmptySprite;
    }

    private bool FindCookData(CookData cook)
    {
        if (needLootList.Count == cook.needLoots.Length)
        {
            //개수가 맞아야 재료 매칭 시작
            for (int i = 0; i < cook.needLoots.Length; i++)
            {
                bool isLootMatch = false;
                //매칭해서 확인한것은 건너뜀
                for (int j = 0; j < needLootList.Count; j++)
                {
                    //cook data에 있는 필요 재료 리스트와 현재 쿠커 재료리스트를 각각 매칭하면서 존재하는지 확인
                    if (needLootList[j].lootdata == cook.needLoots[i].lootdata)
                    {
                        //종류가 맞긴한데 개수가 안맞는 경우
                        if (needLootList[j].count != cook.needLoots[i].count)
                            isLootMatch = false;
                        //개수가 맞으면 다음 재료 매칭
                        else
                            isLootMatch = true;
                    }
                }
                //재료매칭 안됨
                if(!isLootMatch)
                    return false;
            }
            //for문이 끝났다는 것은 모두 있는것. 요리를 찾았으니 참을 반환한다.
            cookImage.sprite = cook.sprite;
            return true;
        }
        //개수가 맞지 않으면 바로 다음요리 매칭
        return false;
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

        Cook();
    }

    private void Cook()
    {

    }

    private void ResetCooker()
    {
        for (int i = 0; i < lootList.Count; i++)
        {
            InventoryManager.Instance.GetLoots(lootList[i]);
        }

        lootList.Clear();
        needLootList.Clear();
        cookImage.sprite = cookEmptySprite;
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
        needLootList.Clear();
        cookImage.sprite = cookEmptySprite;
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
