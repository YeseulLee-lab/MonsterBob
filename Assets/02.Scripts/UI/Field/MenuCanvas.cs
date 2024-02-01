using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCanvas : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject menuPopup;
    [SerializeField] private Button backLobbyButton;
    [SerializeField] private Button cancelButton;

    private void OnEnable()
    {
        menuPopup.transform.localScale = Vector2.zero;

        menuPopup.transform.DOScale(1f, 0.3f).SetEase(Ease.InExpo).OnComplete(() =>
        {
            Time.timeScale = 0;
        });
    }

    private void Start()
    {
        backLobbyButton.onClick.AddListener(delegate
        {
            AudioManager.instance.PlaySound("ButtonClick2");
            LoadingManager.Instance.LoadNextScene("02.Lobby");
        });

        cancelButton.onClick.AddListener(delegate
        {
            AudioManager.instance.PlaySound("UIClose");
            Time.timeScale = 1;
            menuPopup.transform.DOScale(0f, 0.3f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        });
    }
}
