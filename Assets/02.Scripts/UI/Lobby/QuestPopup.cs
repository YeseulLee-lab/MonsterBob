using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPopup : MonoBehaviour
{
    [SerializeField] private Button background;
    [SerializeField] private Button questButton;
    [SerializeField] private RectTransform questPanel;

    private void Start()
    {
        questButton.onClick.AddListener(delegate
        {
            AudioManager.instance.PlaySound("Swipe");
            questPanel.DOMoveX(0f, 0.3f).SetEase(Ease.Linear);
            background.gameObject.SetActive(true);
        });

        background.onClick.AddListener(delegate
        {
            questPanel.DOMoveX(-questPanel.rect.width, 0.3f).SetEase(Ease.Linear);
            background.gameObject.SetActive(false);
        });
    }
}
