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
                //�丮�� ã�Ƽ� ���̻� ��Ī���ʿ䰡����
                return;
            }
        }
        //��� �������� �丮�� ����.
        cookImage.sprite = cookEmptySprite;
    }

    private bool FindCookData(CookData cook)
    {
        if (needLootList.Count == cook.needLoots.Length)
        {
            //������ �¾ƾ� ��� ��Ī ����
            for (int i = 0; i < cook.needLoots.Length; i++)
            {
                bool isLootMatch = false;
                //��Ī�ؼ� Ȯ���Ѱ��� �ǳʶ�
                for (int j = 0; j < needLootList.Count; j++)
                {
                    //cook data�� �ִ� �ʿ� ��� ����Ʈ�� ���� ��Ŀ ��Ḯ��Ʈ�� ���� ��Ī�ϸ鼭 �����ϴ��� Ȯ��
                    if (needLootList[j].lootdata == cook.needLoots[i].lootdata)
                    {
                        //������ �±��ѵ� ������ �ȸ´� ���
                        if (needLootList[j].count != cook.needLoots[i].count)
                            isLootMatch = false;
                        //������ ������ ���� ��� ��Ī
                        else
                            isLootMatch = true;
                    }
                }
                //����Ī �ȵ�
                if(!isLootMatch)
                    return false;
            }
            //for���� �����ٴ� ���� ��� �ִ°�. �丮�� ã������ ���� ��ȯ�Ѵ�.
            cookImage.sprite = cook.sprite;
            return true;
        }
        //������ ���� ������ �ٷ� �����丮 ��Ī
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
        resetButton.GetComponentInChildren<Text>().text = "������";

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
        resetButton.GetComponentInChildren<Text>().text = "�ʱ�ȭ";

        clock.gameObject.SetActive(false);

        cookerAnimator.enabled = false;
    }
}
