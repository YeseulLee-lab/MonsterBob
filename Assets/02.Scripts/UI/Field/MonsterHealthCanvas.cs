using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthCanvas : MonoBehaviour
{
    [SerializeField] private Button exitButton;

    private void Start()
    {
        exitButton.onClick.AddListener(ExitFromClickerMode);
    }

    private void ExitFromClickerMode()
    {
        FieldManager.Instance.SwitchCamera(false, null);
    }
}
