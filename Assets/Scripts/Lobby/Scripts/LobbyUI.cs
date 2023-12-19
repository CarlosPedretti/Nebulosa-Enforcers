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

public class LobbyUI : NetworkBehaviour
{


    public static LobbyUI Instance { get; private set; }


    [SerializeField] private Transform playerSingleTemplate;
    [SerializeField] private Transform container;
    [SerializeField] private Transform canvas;
    [SerializeField] private Transform prefabSelectionButtons;
    [SerializeField] private Transform countDownContainer;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private TextMeshProUGUI gameModeText;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Button changeP1_Button;
    [SerializeField] private Button changeP2_Button;
    [SerializeField] private Button changeP3_Button;
    [SerializeField] private Button changeP4_Button;
    [SerializeField] private Button leaveLobbyButton;
    [SerializeField] private Button changeGameModeButton;
    [SerializeField] private Button startRelayButton;
    [SerializeField] private Button startGameButton;

    public NetworkVariable<bool> canUseStartRelayAndChangeGameMode = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> canHideCanvas = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> showCountDown = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> countDown = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);



    private void Awake()
    {

        Instance = this;

        playerSingleTemplate.gameObject.SetActive(false);

        changeP1_Button.onClick.AddListener(() =>
        {
            LobbyManager.Instance.UpdatePlayerCharacter(LobbyManager.PlayerCharacter.Asset1);
            ButtonSetPrefabPlayer(0);
        });
        changeP2_Button.onClick.AddListener(() =>
        {
            LobbyManager.Instance.UpdatePlayerCharacter(LobbyManager.PlayerCharacter.Asset2);
            ButtonSetPrefabPlayer(1);
        });
        changeP3_Button.onClick.AddListener(() =>
        {
            LobbyManager.Instance.UpdatePlayerCharacter(LobbyManager.PlayerCharacter.Asset3);
            ButtonSetPrefabPlayer(2);
        });
        changeP4_Button.onClick.AddListener(() =>
        {
            LobbyManager.Instance.UpdatePlayerCharacter(LobbyManager.PlayerCharacter.Asset4);
            ButtonSetPrefabPlayer(3);
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
            StartCoroutine(WaitAndShowCountdown(5));
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

        if (showCountDown != null)
        {
            showCountDown.Value = false;
        }
    }

    private void Start()
    {

        NetworkSpawn();

        LobbyManager.Instance.OnJoinedLobby += UpdateLobby_Event;
        LobbyManager.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;
        LobbyManager.Instance.OnLobbyGameModeChanged += UpdateLobby_Event;
        LobbyManager.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;
        LobbyManager.Instance.OnKickedFromLobby += LobbyManager_OnLeftLobby;

        Hide();
    }

    private void LobbyManager_OnLeftLobby(object sender, System.EventArgs e)
    {
        ClearLobby();
        Hide();
    }

    private void UpdateLobby_Event(object sender, LobbyManager.LobbyEventArgs e)
    {
        UpdateLobby();
    }

    private void UpdateLobby()
    {
        UpdateLobby(LobbyManager.Instance.GetJoinedLobby());
    }

    private void UpdateLobby(Lobby lobby)
    {
        ClearLobby();

        foreach (Player player in lobby.Players)
        {
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


        if (showCountDown.Value)
        {
            countDownContainer.gameObject.SetActive(true);
        }
        else
        {
            countDownContainer.gameObject.SetActive(false);
        }



        lobbyNameText.text = lobby.Name;
        playerCountText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;
        gameModeText.text = lobby.Data[LobbyManager.KEY_GAME_MODE].Value;

        Show();
    }

    private void ClearLobby()
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

    public void ShowCharactersButton()
    {
        canUseStartRelayAndChangeGameMode.Value = false;
    }

    public void HideCanvas()
    {
        canHideCanvas.Value = true;
    }

    public void ShowCountdown()
    {
        showCountDown.Value = true;
    }

    public void HideCountdown()
    {
        showCountDown.Value = false;
    }



    public void ButtonSetPrefabPlayer(int prefabID)
    {
        NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<SpawnPlayer>().SetPlayerPrefabServerRpc(NetworkManager.Singleton.LocalClientId, prefabID);
    }


    IEnumerator WaitAndShowCountdown(int waitTime)
    {
        ShowCountdown();

        for (int i = waitTime; i > 0; i--)
        {
            countDown.Value = i;
            countdownText.text = countDown.Value.ToString();

            yield return new WaitForSeconds(1f);
        }


        HideCanvas();
        HideCountdown();

        LobbyManager.Instance.StartGame();
    }

    private void CountDownChanged(int oldValue, int newValue)
    {
        countdownText.text = newValue.ToString();
    }

    void OnEnable()
    {
        countDown.OnValueChanged += CountDownChanged;
    }

    void OnDisable()
    {
        countDown.OnValueChanged -= CountDownChanged;

    }

}