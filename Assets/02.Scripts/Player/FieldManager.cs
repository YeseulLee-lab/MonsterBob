using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public static FieldManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

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
    public PlayerState playerState;
}
