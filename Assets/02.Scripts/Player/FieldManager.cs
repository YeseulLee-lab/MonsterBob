using Cinemachine;
using DG.Tweening;
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

        AudioManager.instance.PlayMusic(fieldAudioClip);
    }

    [Header("Audio")]
    [SerializeField] private AudioClip fieldAudioClip;

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCam;
    [SerializeField] private CinemachineVirtualCamera monsterVirtualCam;

    public CinemachineVirtualCamera MonsterVirtualCam
    {
        get
        {
            return monsterVirtualCam;
        }
    }

    [Header("MonsterSpawn")]
    [SerializeField] private MonsterPatrol[] monsters;
    private Dictionary<MonsterPatrol, int> spawnedMonsters;
    [SerializeField] private int spawnMaxCount;
    [SerializeField] private int eachSpawnMaxCount;

    public bool isClickerMode = false;

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

    private void Start()
    {
        SpawnMonster();
    }

    public void SetCameraPOV(float pov, float duration)
    {
        //virtualCam.m_Lens.FieldOfView = pov;
        DOVirtual.Float(virtualCam.m_Lens.FieldOfView, pov, duration, (value) =>
        {
            virtualCam.m_Lens.FieldOfView = value;
        });
    }

    public void SwitchCamera(bool isAttacking, Transform target)
    {
        if (isAttacking)
        {
            virtualCam.gameObject.SetActive(false);
            monsterVirtualCam.gameObject.SetActive(true);
            monsterVirtualCam.m_LookAt = target;
            monsterVirtualCam.m_Follow = target;
            UIManager.Instance.SetClickerModeCanvas();
        }
        else
        {
            virtualCam.gameObject.SetActive(true);
            monsterVirtualCam.gameObject.SetActive(false);
            UIManager.Instance.ExitClickerModeCanvas();
        }
        isClickerMode = isAttacking;
    }

    public void SpawnMonster()
    {

    }
}
