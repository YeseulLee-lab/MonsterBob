using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthCanvas : MonoBehaviour
{
    [SerializeField] private Text guideText;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        exitButton.onClick.AddListener(ExitFromClickerMode);
    }

    private void OnEnable()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(guideText.transform.DOScale(2f, 1f).SetEase(Ease.InBounce)).Append(guideText.transform.DOScale(1f, 1f).SetEase(Ease.OutBounce));
    }

    private void ExitFromClickerMode()
    {
        FieldManager.Instance.SwitchCamera(false, null);
    }
}
