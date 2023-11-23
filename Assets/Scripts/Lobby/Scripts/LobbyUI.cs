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

public class LobbyUI : NetworkBehaviour {


    public static LobbyUI Instance { get; private set; }


    [SerializeField] private Transform playerSingleTemplate;
    [SerializeField] private Transform container;
    [SerializeField] private Transform canvas;
    [SerializeField] private Transform prefabSelectionButtons;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private TextMeshProUGUI gameModeText;
    [SerializeField] private Button changeMarineButton;
    [SerializeField] private Button changeNinjaButton;
    [SerializeField] private Button changeZombieButton;
    [SerializeField] private Button leaveLobbyButton;
    [SerializeField] private Button changeGameModeButton;
    [SerializeField] private Button startRelayButton;
    [SerializeField] private Button startGameButton;

    public NetworkVariable<bool> canUseStartRelayAndChangeGameMode = new NetworkVariable<bool>(true);
    public NetworkVariable<bool> canHideCanvas = new NetworkVariable<bool>(false);


    private void Awake()
    {

        Instance = this;

        playerSingleTemplate.gameObject.SetActive(false);

        changeMarineButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.UpdatePlayerCharacter(LobbyManager.PlayerCharacter.Asset1, LobbyManager.PlayerPrefab.Prefab1);
            //NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerLogic>().SetPlayerPrefabServerRpc(NetworkManager.Singleton.LocalClientId, 0);
            ButtonSetPrefabPlayer(0);
        });
        changeNinjaButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.UpdatePlayerCharacter(LobbyManager.PlayerCharacter.Asset2, LobbyManager.PlayerPrefab.Prefab2);
            //NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerLogic>().SetPlayerPrefabServerRpc(NetworkManager.Singleton.LocalClientId, 1);
            ButtonSetPrefabPlayer(1);
        });
        changeZombieButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.UpdatePlayerCharacter(LobbyManager.PlayerCharacter.Asset3, LobbyManager.PlayerPrefab.Prefab3);
            //NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerLogic>().SetPlayerPrefabServerRpc(NetworkManager.Singleton.LocalClientId, 2);
            ButtonSetPrefabPlayer(2);
        });

        leaveLobbyButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.LeaveLobby();
        });

        changeGameModeButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.ChangeGameMode();
        });

        startRelayButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.StartRelay();
        });

        startGameButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.StartGame();
        });


    }


    private void NetworkSpawn()
    {
        if (canUseStartRelayAndChangeGameMode != null)
        {
            canUseStartRelayAndChangeGameMode.Value = true;
        }

        if (canHideCanvas != null)
        {
            canHideCanvas.Value = false;
        }
    }

private void Start() {

        NetworkSpawn();

        LobbyManager.Instance.OnJoinedLobby += UpdateLobby_Event;
        LobbyManager.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;
        LobbyManager.Instance.OnLobbyGameModeChanged += UpdateLobby_Event;
        LobbyManager.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;
        LobbyManager.Instance.OnKickedFromLobby += LobbyManager_OnLeftLobby;

        Hide();
    }

    private void LobbyManager_OnLeftLobby(object sender, System.EventArgs e) {
        ClearLobby();
        Hide();
    }

    private void UpdateLobby_Event(object sender, LobbyManager.LobbyEventArgs e) {
        UpdateLobby();
    }

    private void UpdateLobby() {
        UpdateLobby(LobbyManager.Instance.GetJoinedLobby());
    }

    private void UpdateLobby(Lobby lobby) {
        ClearLobby();

        foreach (Player player in lobby.Players) {
            Transform playerSingleTransform = Instantiate(playerSingleTemplate, container);
            playerSingleTransform.gameObject.SetActive(true);
            LobbyPlayerSingleUI lobbyPlayerSingleUI = playerSingleTransform.GetComponent<LobbyPlayerSingleUI>();

            lobbyPlayerSingleUI.SetKickPlayerButtonVisible(
                LobbyManager.Instance.IsLobbyHost() &&
                player.Id != AuthenticationService.Instance.PlayerId // Don't allow kick self
            );

            lobbyPlayerSingleUI.UpdatePlayer(player);

        }

        if (canUseStartRelayAndChangeGameMode.Value)
        {
            if (LobbyManager.Instance.IsLobbyHost())
            {
                changeGameModeButton.gameObject.SetActive(true);
                startRelayButton.gameObject.SetActive(true);
            }
            else
            {
                changeGameModeButton.gameObject.SetActive(false);
                startRelayButton.gameObject.SetActive(false);
            }
        }
        else
        {

            if (LobbyManager.Instance.IsLobbyHost())
            {
                changeGameModeButton.gameObject.SetActive(false);
                startRelayButton.gameObject.SetActive(false);
                prefabSelectionButtons.gameObject.SetActive(true);
            }
            else
            {
                prefabSelectionButtons.gameObject.SetActive(true);
                startGameButton.gameObject.SetActive(false);
                changeGameModeButton.gameObject.SetActive(false);
                startRelayButton.gameObject.SetActive(false);
            }
        }

        if (canHideCanvas.Value)
        {
            canvas.gameObject.SetActive(false);
        }


        lobbyNameText.text = lobby.Name;
        playerCountText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;
        gameModeText.text = lobby.Data[LobbyManager.KEY_GAME_MODE].Value;

        Show();
    }

    private void ClearLobby() {
        foreach (Transform child in container) {
            if (child == playerSingleTemplate) continue;
            Destroy(child.gameObject);
        }
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    public void ShowCharactersButton()
    {
        canUseStartRelayAndChangeGameMode.Value = false;
    }

    public void HideCanvas()
    {
        canHideCanvas.Value = true;
    }


    public void ButtonSetPrefabPlayer(int prefabID)
    {
        NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerLogic>().SetPlayerPrefabServerRpc(NetworkManager.Singleton.LocalClientId, prefabID);
    }

}