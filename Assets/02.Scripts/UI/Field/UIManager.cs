using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public GameObject[] canvases;

    public void SetClickerModeCanvas()
    {
        for (int i = 0; i < canvases.Length; i++)
        {
            if (canvases[i].activeSelf)
            {
                canvases[i].SetActive(false);
            }
        }

        canvases[3].SetActive(true);
    }

    public void ExitClickerModeCanvas()
    {
        canvases[0].SetActive(true);
        canvases[1].SetActive(true);
        canvases[3].SetActive(false);
    }
}
