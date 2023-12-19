using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class SpawnPlayer : NetworkBehaviour
{
    public static SpawnPlayer Instance { get; private set; }

    public NetworkVariable<int> prefabID;
    public NetworkVariable<bool> allPlayersProcessed = new NetworkVariable<bool>(false);


    public GameObject[] prefabs;

    private void Awake()
    {
        Instance = this;
    }


    [ServerRpc]
    public void SetPlayerPrefabServerRpc(ulong pID, int prefabID)
    {
        NetworkManager.ConnectedClients[pID].PlayerObject.GetComponent<SpawnPlayer>().prefabID.Value = prefabID;
    }


    [ServerRpc]
    public void SpawnOtherPlayersServerRpc()
    {
        foreach (ushort pID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (NetworkManager.Singleton.ConnectedClientsList[pID].PlayerObject.GetComponent<SpawnPlayer>().prefabID.Value == 0)
            {
                GameObject newPrefab = Instantiate(prefabs[0]);
                newPrefab.GetComponent<NetworkObject>().SpawnWithOwnership(pID);
                Debug.Log("SpawnOtherPlayersServerRpc EJECUTADO! " + prefabID.Value);
            }
            else if (NetworkManager.Singleton.ConnectedClientsList[pID].PlayerObject.GetComponent<SpawnPlayer>().prefabID.Value == 1)
            {
                GameObject newPrefab = Instantiate(prefabs[1]);
                newPrefab.GetComponent<NetworkObject>().SpawnWithOwnership(pID);
                Debug.Log("SpawnOtherPlayersServerRpc EJECUTADO! " + prefabID.Value);
            }
            else if (NetworkManager.Singleton.ConnectedClientsList[pID].PlayerObject.GetComponent<SpawnPlayer>().prefabID.Value == 2)
            {
                GameObject newPrefab = Instantiate(prefabs[2]);
                newPrefab.GetComponent<NetworkObject>().SpawnWithOwnership(pID);
                Debug.Log("SpawnOtherPlayersServerRpc EJECUTADO! " + prefabID.Value);
            }
            else if (NetworkManager.Singleton.ConnectedClientsList[pID].PlayerObject.GetComponent<SpawnPlayer>().prefabID.Value == 3)
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

        if (allPlayersProcessed.Value == true)
         {
             if (IsServer)
             {
                 DespawnPlayerSpawnServerRpc();
             }
             else
             {
                 DespawnPlayerSpawnClientRpc();
             }
         }

    }


    [ServerRpc]
    private void DespawnPlayerSpawnServerRpc()
    {
        foreach (ushort pID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            NetworkObject networkObject = NetworkManager.Singleton.ConnectedClients[pID].PlayerObject.GetComponent<NetworkObject>();

            if (networkObject != null)
            {
                SpawnPlayer spawnPlayerComponent = networkObject.GetComponent<SpawnPlayer>();

                if (spawnPlayerComponent != null)
                {
                    networkObject.Despawn();
                    Destroy(networkObject);

                    Debug.Log("Despawn desde ServerRPC");
                }
                else
                {
                    Debug.Log("SpawnPlayer no encontrado en el objeto del jugador");
                }
            }
            else
            {
                Debug.Log("NetworkObject nulo en el objeto del jugador ServerRPC");
            }
        }
    }


    [ClientRpc]
    private void DespawnPlayerSpawnClientRpc()
    {
        foreach (ushort pID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            NetworkObject networkObject = NetworkManager.Singleton.ConnectedClients[pID].PlayerObject.GetComponent<NetworkObject>();

            if (networkObject != null)
            {
                SpawnPlayer spawnPlayerComponent = networkObject.GetComponent<SpawnPlayer>();
                if (spawnPlayerComponent != null)
                {
                    networkObject.Despawn();
                    Destroy(networkObject);

                    Debug.Log("Despawn desde Client");
                }
                else
                {
                    Debug.Log("SpawnPlayer no encontrado en el objeto del jugador ClientRPC");
                }
            }
            else
            {
                Debug.Log("NetworkObject nulo dentro del client ");
            }
        }
    }

}
