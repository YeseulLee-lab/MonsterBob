using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCanvas : MonoBehaviour
{
    [SerializeField] private LobbyUICanvas lobbyUICanvas;

    [Header("Buttons")]
    [SerializeField] private Button backButton;

    private void Start()
    {
        backButton.onClick.AddListener(delegate
        {
            gameObject.SetActive(false);
        });
    }
}
