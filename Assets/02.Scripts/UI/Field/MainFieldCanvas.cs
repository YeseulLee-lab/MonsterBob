using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [Header("Inventory")]
    [SerializeField] private RectTransform getItemTransform;
    public Image backPackImage;

    private void Start()
    {
        attackButton.onClick.AddListener(delegate
        {
            if(FieldManager.Instance.isClickerMode)
                return;
            FieldManager.Instance.playerState = FieldManager.PlayerState.AttackSword;
        });

        menuButton.onClick.AddListener(delegate
        {
            AudioManager.instance.PlaySound("ButtonClick2");
            UIManager.Instance.canvases[2].SetActive(true);
        });

        isTimerOn = true;
    }


    private void Update()
    {
        if (isTimerOn)
        {
            if(FieldManager.Instance.isClickerMode)
                return;

            if (timerCount > 0)
            {
                timerCount -= Time.deltaTime;
                TimerUpdate(timerCount);
            }
            else
            {
                timerCount = 0;
                isTimerOn = false;
                SceneManager.LoadScene("02.Lobby");
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
