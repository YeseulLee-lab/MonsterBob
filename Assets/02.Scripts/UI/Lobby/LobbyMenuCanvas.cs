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
            AudioManager.instance.PlaySound("OpenMap");
            lobbyUICanvas.mapCanvas.SetActive(true);
        });

        bagButton.onClick.AddListener(delegate
        {
            AudioManager.instance.PlaySound("ButtonClick2");
            lobbyUICanvas.bagCanvas.SetActive(true);
        });
    }
}
