using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using QFSW.QC;

public class TestLobby : MonoBehaviour
{
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    [Command]
    private async void CreateLobby()
    {
        try
        {
            string LobbyName = "MyLobby";
            int maxPlayers = 4;

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(LobbyName, maxPlayers);

            Debug.Log("Created lobby " + lobby.Name + " " + lobby.MaxPlayers);
        }
        catch(LobbyServiceException error)
        {
            Debug.Log(error);
        }
    }

}
