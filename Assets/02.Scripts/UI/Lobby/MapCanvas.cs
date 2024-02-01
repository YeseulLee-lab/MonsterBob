using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapCanvas : MonoBehaviour
{
    [SerializeField] private LobbyUICanvas lobbyUICanvas;

    [Header("Buttons")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button forestLandButton;
    [SerializeField] private Button skyLandButton;
    [SerializeField] private Button seaLandButton;

    [Header("MapInfo")]
    [SerializeField] private GameObject mapInfoBackground;
    [SerializeField] private GameObject mapInfoPanel;
    [SerializeField] private Text landName;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button enterButton;
    [SerializeField] private MonsterPatrol[] monsters;
    [SerializeField] private MonsterSlot[] monsterSlots;

    public enum LandType
    {
        forest, sky, sea
    }
    private LandType landType;

    private void OnEnable()
    {
        transform.DOScale(1f, 0.5f).SetEase(Ease.InExpo);
    }

    private void Start()
    {
        backButton.onClick.AddListener(delegate
        {
            AudioManager.instance.PlaySound("UIClose");
            transform.DOScale(0f, 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        });

        cancelButton.onClick.AddListener(delegate
        {
            AudioManager.instance.PlaySound("UIClose");
            mapInfoPanel.transform.DOScale(0f, 0.3f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                mapInfoBackground.SetActive(false);
            });
        });

        forestLandButton.onClick.AddListener(() => 
        {
            AudioManager.instance.PlaySound("ButtonClick3");
            landType = LandType.forest;
            SetMapInfoPanel(landType);
        });
        skyLandButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySound("ButtonClick3");
            landType = LandType.sky;
            SetMapInfoPanel(landType);
        });
        seaLandButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySound("ButtonClick3");
            landType = LandType.sea;
            SetMapInfoPanel(landType);
        });

        enterButton.onClick.AddListener(OnClickEnter);
    }

    private void SetMapInfoPanel(LandType landType)
    {
        switch(landType)
        {
            case LandType.forest:
                landName.text = "���� ��\n~���� Ǯ�� ������ ��~";
                break;
            case LandType.sky:
                landName.text = "�ϴ��� ��\n~����Ⱑ Ȱ���� ġ�� ��~";
                break;
            case LandType.sea:
                landName.text = "�ٴ��� ��\n~������ �������� ���� �ߴ� ��~";
                break;
        }

        List<MonsterPatrol> landMonsters = new List<MonsterPatrol>();
        foreach (MonsterPatrol monster in monsters)
        {
            if(monster.monster.landType == landType)
                landMonsters.Add(monster);
        }

        for (int i = 0; i < landMonsters.Count; i++)
        {
            monsterSlots[i].gameObject.SetActive(true);
            monsterSlots[i].monsterImage.sprite = landMonsters[i].monster.sprite;
        }

        mapInfoBackground.SetActive(true);
        mapInfoPanel.transform.DOScale(1f, 0.3f).SetEase(Ease.InExpo);
    }

    private void OnClickEnter()
    {
        //��� ȿ����
        LoadingManager.Instance.LoadNextScene("04.Field");
    }
}
