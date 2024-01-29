using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUIManager : MonoBehaviour
{
    public static FieldUIManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public VariableJoystick joystick;
    public Button attackButton;

    [Header("Timer")]
    [SerializeField] private Text timer;
    [SerializeField] private float timerCount;

    [Header("Menu")]
    [SerializeField] private Button menuButton;
    [SerializeField] private GameObject menuPopup;

    public enum PlayerState
    {
        Idle,
        Walk,
        AttackSword,
        AttackBow,
        AttackSpear,
        Damage,
        Dead,
    }
    public PlayerState state;

    private void Start()
    {
        attackButton.onClick.AddListener(delegate
        {
            state = PlayerState.AttackSword;
        });
    }

    private void TimerUpdate()
    {

    }
}
