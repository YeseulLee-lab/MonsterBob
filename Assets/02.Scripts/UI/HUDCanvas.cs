using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDCanvas : MonoBehaviour
{
    [SerializeField] private Text playerHealthText;
    [SerializeField] private Image healthBar;

    public void HealthBarUpdate(float playerCurHealth)
    {
        healthBar.fillAmount = playerCurHealth / 100;
        playerHealthText.text = string.Format("{0} / {1}", playerCurHealth, 100);
    }
}
