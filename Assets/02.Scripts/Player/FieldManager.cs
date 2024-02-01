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
    public Camera cam;

    [Header("MonsterSpawn")]
    [SerializeField] private MonsterPatrol[] monsters;
    private Dictionary<MonsterPatrol, int> spawnedMonsters;
    [SerializeField] private int spawnMaxCount;
    [SerializeField] private int eachSpawnMaxCount;
    

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
        cam = Camera.main;

        SpawnMonster();
    }

    public void SetCameraPOV(float pov, float duration)
    {
        virtualCam.m_Lens.FieldOfView = pov;
        //DOVirtual.Float(virtualCam.m_Lens.FieldOfView, pov, duration, null);
    }

    public void SpawnMonster()
    {

    }
}
