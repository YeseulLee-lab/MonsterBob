using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenuCanvas : MonoBehaviour
{
    [SerializeField] private LobbyUICanvas lobbyUICanvas;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button bookButton;


    private void Start()
    {
        playButton.onClick.AddListener(delegate
        {
            lobbyUICanvas.mapCanvas.SetActive(true);
        });
    }
}
