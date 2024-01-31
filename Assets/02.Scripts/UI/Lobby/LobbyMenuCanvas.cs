using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenuCanvas : MonoBehaviour
{
    [SerializeField] private LobbyUICanvas lobbyUICanvas;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button bagButton;


    private void Start()
    {
        playButton.onClick.AddListener(delegate
        {
            lobbyUICanvas.mapCanvas.SetActive(true);
        });

        bagButton.onClick.AddListener(delegate
        {
            lobbyUICanvas.bagCanvas.SetActive(true);
        });
    }
}
