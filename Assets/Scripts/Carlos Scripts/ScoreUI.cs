using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using Unity.Netcode.Transports.UTP;

public class ScoreUI : NetworkBehaviour
{
    public static ScoreUI Instance { get; private set; }


    [SerializeField] private Transform playerSingleTemplate;
    [SerializeField] private Transform container;
    [SerializeField] private Transform hostButtons;
    [SerializeField] private Button leaveLobbyButton;
    [SerializeField] private Button restartGameButton;

    private void Awake()
    {

        Instance = this;

        playerSingleTemplate.gameObject.SetActive(false);
    }

    public void UpdateScore()
    {
        ClearScore();

        PlayerLogic[] players = FindObjectsOfType<PlayerLogic>();

        foreach (PlayerLogic player in players)
        {
            Transform playerSingleTransform = Instantiate(playerSingleTemplate, container);
            playerSingleTransform.gameObject.SetActive(true);
            ScorePlayerSingleUI scorePlayerSingleUI = playerSingleTransform.GetComponent<ScorePlayerSingleUI>();

            scorePlayerSingleUI.UpdatePlayer(player);
        }

        //lobbyNameText.text = lobby.Name;
        //playerCountText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;
        //gameModeText.text = lobby.Data[LobbyManager.KEY_GAME_MODE].Value;

        Show();
    }

    private void ClearScore()
    {
        foreach (Transform child in container)
        {
            if (child == playerSingleTemplate) continue;
            Destroy(child.gameObject);
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }


}