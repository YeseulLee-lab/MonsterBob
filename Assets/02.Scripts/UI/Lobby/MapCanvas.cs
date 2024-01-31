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

    private enum LandType
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
            transform.DOScale(0f, 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        });

        cancelButton.onClick.AddListener(delegate
        {
            mapInfoPanel.transform.DOScale(0f, 0.3f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                mapInfoBackground.SetActive(false);
            });
        });

        forestLandButton.onClick.AddListener(() => SetMapInfoPanel(LandType.forest));
        skyLandButton.onClick.AddListener(() => SetMapInfoPanel(LandType.sky));
        seaLandButton.onClick.AddListener(() => SetMapInfoPanel(LandType.sea));

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
        mapInfoBackground.SetActive(true);
        mapInfoPanel.transform.DOScale(1f, 0.3f).SetEase(Ease.InExpo);
    }

    private void OnClickEnter()
    {
        LoadingManager.Instance.LoadNextScene("04.Field");
    }
}
