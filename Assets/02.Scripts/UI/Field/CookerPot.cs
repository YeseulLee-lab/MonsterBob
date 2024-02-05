using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CookerPot : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image cookerImage;
    public void OnDrop(PointerEventData eventData)
    {
        AudioManager.instance.PlaySound("LowPop");

        Sequence sequence = DOTween.Sequence();
        sequence.Append(cookerImage.transform.DOScale(1.1f, 0.3f)).Append(cookerImage.transform.DOScale(1f, 0.3f));
    }
}
