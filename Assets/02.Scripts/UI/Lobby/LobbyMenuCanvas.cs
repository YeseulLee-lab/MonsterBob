using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenuCanvas : MonoBehaviour
{
    [SerializeField] private LobbyUICanvas lobbyUICanvas;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button cookButton;
    [SerializeField] private Button bookButton;


    private void Start()
    {
        playButton.onClick.AddListener(delegate
        {
            AudioManager.instance.PlaySound("OpenMap");
            lobbyUICanvas.mapCanvas.SetActive(true);
        });
        cookButton.onClick.AddListener(delegate
        {
            AudioManager.instance.PlaySound("ButtonClick2");
            lobbyUICanvas.cookCanvas.SetActive(true);
        });
        bookButton.onClick.AddListener(delegate
        {
            AudioManager.instance.PlaySound("ButtonClick2");
            lobbyUICanvas.bookCanvas.SetActive(true);
        });
    }
}
