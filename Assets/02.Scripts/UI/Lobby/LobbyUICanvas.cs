using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUICanvas : MonoBehaviour
{
    [SerializeField] private AudioClip lobbyBGM;
    public GameObject mapCanvas;
    public GameObject lobbyMenuCanvas;
    public GameObject bagCanvas;

    private void Start()
    {
        AudioManager.instance.PlayMusic(lobbyBGM, 0.5f);
    }
}
