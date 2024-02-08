using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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

    [Header("Timer")]
    [SerializeField] private Text timer;
    private bool isTimerOn;

    [Header("Buttons")]
    [SerializeField] private Button cookButton;
    [SerializeField] private Button resetButton;

    private CookData[] cookDatas;
    private float cookTime;
    

    private void Start()
    {
        cookButton.onClick.AddListener(OnClickCookButton);

        resetButton.onClick.AddListener(ResetCooker);

        cookDatas = cookCanvas.cookDatas;
    }

    private void Update()
    {
        if (isTimerOn)
        {
            if (cookTime > 0)
            {
                cookTime -= Time.deltaTime;
                TimerUpdate(cookTime);
            }
            else
            {
                cookTime = 0;
                isTimerOn = false;
                FinishCook();
            }
        }
    }

    private void TimerUpdate(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timer.text = string.Format("{0:00} : {1:00}", minutes, seconds);
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
            cookTime = cook.cookTime;
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
        isTimerOn = true;
        cookerAnimator.SetBool("isCooking", true);
        clock.gameObject.SetActive(true);

        cookButton.interactable = false;
        resetButton.onClick.RemoveAllListeners();
        resetButton.onClick.AddListener(ThrowAway);
        resetButton.GetComponentInChildren<Text>().text = "������";
    }

    private void FinishCook()
    {
        cookerAnimator.SetBool("isCooking", false);
        cookImage.GetComponent<Animator>().SetBool("isFinished", true);
        clock.gameObject.SetActive(false);

        cookImage.GetComponent<Button>().onClick.AddListener(delegate
        {
            cookImage.GetComponent<Animator>().SetBool("isFinished", false);
            resetButton.onClick.RemoveAllListeners();
            resetButton.onClick.AddListener(ResetCooker);
            resetButton.GetComponentInChildren<Text>().text = "�ʱ�ȭ";
            lootList.Clear();
            needLootList.Clear();
            for (int i = 0; i < lootImageArr.Length; i++)
            {
                lootImageArr[i].enabled = false;
                lootImageArr[i].sprite = null;
            }

            Transform originParent = cookImage.transform.parent;
            cookImage.transform.SetParent(cookCanvas.slotText.transform);
            cookImage.transform.DOLocalMove(Vector3.zero, 0.5f).OnComplete(() =>
            {
                cookImage.sprite = cookEmptySprite;
                cookImage.transform.SetParent(originParent);
                cookImage.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                cookImage.GetComponent<Button>().onClick.RemoveAllListeners();
            });
        });
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
        isTimerOn = false;
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

        cookerAnimator.SetBool("isCooking", false);
        cookImage.GetComponent<Animator>().SetBool("isFinished", false);
    }
}
