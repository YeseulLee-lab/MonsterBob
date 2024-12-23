using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartCanvas : MonoBehaviour
{
    [Header("Start Background")]
    [SerializeField] private CanvasGroup backgroundCV;
    [SerializeField] private Button backgroundButton;

    [SerializeField] private AudioClip startBGM;

    private void Start()
    {
        backgroundButton.onClick.AddListener(BackgroundStart);
        AudioManager.instance.PlayMusic(startBGM, 0.5f);
    }

    private void BackgroundStart()
    {
        backgroundCV.DOFade(0f, 1.5f).OnComplete(() =>
        {
            LoadingManager.Instance.LoadNextScene("02.Lobby");
        });
    }
}
