using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSlot : MonoBehaviour
{
    public Image monsterImage;

    private void OnDisable()
    {
        gameObject.SetActive(false);
    }
}
