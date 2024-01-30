using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainFieldCanvas : MonoBehaviour
{
    public VariableJoystick joystick;
    public Button attackButton;

    [Header("Timer")]
    [SerializeField] private Text timer;
    [SerializeField] private float timerCount;
    private bool isTimerOn;

    [Header("Menu")]
    [SerializeField] private Button menuButton;
    [SerializeField] private GameObject menuPopup;

    [Header("Inventory")]
    [SerializeField] private RectTransform getItemTransform;

    private void Start()
    {
        attackButton.onClick.AddListener(delegate
        {
            FieldManager.Instance.playerState = FieldManager.PlayerState.AttackSword;
        });

        isTimerOn = true;
    }


    private void Update()
    {
        if (isTimerOn)
        {
            if (timerCount > 0)
            {
                timerCount -= Time.deltaTime;
                TimerUpdate(timerCount);
            }
            else
            {
                timerCount = 0;
                isTimerOn = false;
            }
        }
    }

    private void TimerUpdate(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timer.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}