using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCanvas : MonoBehaviour
{
    [SerializeField] private Text progressText;

    private void Start()
    {
        LoadingManager.Instance.progressText = this.progressText;
    }
}
