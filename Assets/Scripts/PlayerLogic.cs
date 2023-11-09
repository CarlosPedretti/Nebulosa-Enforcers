using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class PlayerLogic : NetworkBehaviour
{
    public static PlayerLogic Instance { get; private set; }

    public NetworkVariable<int> prefabID;

    private NetworkVariable<bool> allPlayersProcessed = new NetworkVariable<bool>(false);


    public GameObject[] prefabs;

    private void Awake()
    {
        Instance = this;
    }

    /*private void Start()
    {
        NetworkManager.SceneManager.OnSceneEvent += SceneManager_OnSceneEvent;
    }

    private void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
    {
        switch (sceneEvent.SceneEventType)
        {
            case SceneEventType.LoadEventCompleted:
                if (SceneManager.GetActiveScene().name == "Game" && IsOwner)
                {
                    if (IsHost)
                    {
                        //GameManager.Instance.isVan = true;
                        GameObject newCar = Instantiate(prefabs[0], new Vector3(1, 1, -4.5f * 0), Quaternion.identity);
                        newCar.GetComponent<NetworkObject>().SpawnWithOwnership(NetworkManager.LocalClient.ClientId);
                    }
                }
                if (SceneManager.GetActiveScene().name == "GameOver" && IsHost)
                {
                    var carObjects = GameObject.FindGameObjectsWithTag("Car");
                    foreach (GameObject carObject in carObjects)
                    {
                        Destroy(carObject);
                    }
                }
                break;
        }
    }*/



    #region ServerRCP
    [ServerRpc]
    public void SetPlayerPrefabServerRpc(ulong pID, int prefabID)
    {
        NetworkManager.ConnectedClients[pID].PlayerObject.GetComponent<PlayerLogic>().prefabID.Value = prefabID;
    }


    [ServerRpc]
    public void SpawnOtherPlayersServerRpc()
    {
        foreach (ushort pID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            //if (pID != NetworkManager.LocalClientId)

                if (NetworkManager.Singleton.ConnectedClientsList[pID].PlayerObject.GetComponent<PlayerLogic>().prefabID.Value == 0)
                {
                    GameObject newPrefab = Instantiate(prefabs[0]);
                    newPrefab.GetComponent<NetworkObject>().SpawnWithOwnership(pID);
                    Debug.Log("SpawnOtherPlayersServerRpc EJECUTADO! " + prefabID.Value);
                }
                else if (NetworkManager.Singleton.ConnectedClientsList[pID].PlayerObject.GetComponent<PlayerLogic>().prefabID.Value == 1)
                {
                    GameObject newPrefab = Instantiate(prefabs[1]);
                    newPrefab.GetComponent<NetworkObject>().SpawnWithOwnership(pID);
                    Debug.Log("SpawnOtherPlayersServerRpc EJECUTADO! " + prefabID.Value);
                }
                else if (NetworkManager.Singleton.ConnectedClientsList[pID].PlayerObject.GetComponent<PlayerLogic>().prefabID.Value == 2)
                {
                    GameObject newPrefab = Instantiate(prefabs[2]);
                    newPrefab.GetComponent<NetworkObject>().SpawnWithOwnership(pID);
                    Debug.Log("SpawnOtherPlayersServerRpc EJECUTADO! " + prefabID.Value);
                }
                else if (NetworkManager.Singleton.ConnectedClientsList[pID].PlayerObject.GetComponent<PlayerLogic>().prefabID.Value == 3)
                {
                    GameObject newPrefab = Instantiate(prefabs[3]);
                    newPrefab.GetComponent<NetworkObject>().SpawnWithOwnership(pID);
                    Debug.Log("SpawnOtherPlayersServerRpc EJECUTADO! " + prefabID.Value);
                }

            if (pID == NetworkManager.Singleton.ConnectedClientsIds[NetworkManager.Singleton.ConnectedClientsIds.Count - 1])
            {
                allPlayersProcessed.Value = true;
                break;
            }

        }
    }

    /*    [ServerRpc]
    public void GotoSceneServerRPC(string sceneName)
    {
        var status = NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning($"Failed to load {"Game"} " + $"with a {nameof(SceneEventProgressStatus)}: {status}");
        }
    }


       [ServerRpc]
    public void SetPlayerTeamServerRPC(ulong cID, int teamType)
    {
        NetworkManager.ConnectedClients[cID].PlayerObject.GetComponent<PlayerLogic>().prefabID.Value = teamType;
    }

    [ServerRpc]
    public void RestartCarServerRpc(ulong objectID, int nTeam, ulong cID)
    {
        foreach (NetworkObject netObject in NetworkManager.FindObjectsOfType<NetworkObject>())
        {
            if (objectID == netObject.NetworkObjectId)
            {
                netObject.Despawn();
            }
        }

        GameObject newCar = Instantiate(prefabs[nTeam - 1], new Vector3(1, 1, -4.5f * nTeam), Quaternion.identity);
        newCar.GetComponent<NetworkObject>().SpawnWithOwnership(cID, true);
    }

    [ServerRpc]
    public void AddForceServerRpc(ulong clienID, ulong objectID, float var1, float var2, float var3, float var4, float var5, float var6)
    {
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clienID }
            }
        };

        AddForceCarClientRpc(objectID, var1, var2, var3, var4, var5, var6, clientRpcParams);
    }
    #endregion

    #region ClientRCP
    [ClientRpc]
    public void NewClientRpc(int countP)
    {
        //GameManager.Instance.playersCounter = countP;
    }

    [ClientRpc]
    public void AddForceCarClientRpc(ulong objectID, float var1, float var2, float var3, float var4, float var5, float var6, ClientRpcParams clientRpcParams = default)
    {
        foreach (var networkObject in NetworkManager.LocalClient.OwnedObjects)
        {
            if (networkObject.NetworkObjectId == objectID && networkObject.TryGetComponent<Rigidbody>(out Rigidbody rigidCar))
            {
                Debug.Log(networkObject.name);
                Vector3 forceToApply = new Vector3(var1, var2, var3);
                Vector3 positionToApply = new Vector3(var4, var5, var6);
                Debug.Log(forceToApply);
                Debug.Log(positionToApply);
                rigidCar.AddForceAtPosition(forceToApply, positionToApply);
            }
        }
    }*/
    #endregion
}
