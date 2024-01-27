using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoyStickManager : MonoBehaviour
{
    public static JoyStickManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public VariableJoystick joystick;
    public Button attackButton;

    public enum PlayerState
    {
        Idle,
        Walk,
        AttackSword,
        AttackBow,
        AttackSpear,
        Damage,
    }
    public PlayerState state;

    private void Start()
    {
        attackButton.onClick.AddListener(delegate
        {
            state = PlayerState.AttackSword;
        });
    }
}
