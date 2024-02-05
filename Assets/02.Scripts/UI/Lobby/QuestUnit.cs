using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUnit : MonoBehaviour
{
    [SerializeField] private Text questTitle;
    [SerializeField] private Image portrait;
    [SerializeField] private Text charName;
    private CookData[] cookDatas;
    [SerializeField] private QuestCook[] questCooks;
    private int rewardCount;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        
    }

    public void SetData()
    {

    }
}

[Serializable]
public class QuestCook
{
    public Image cookImage;
    public Text cookCount;
}
