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
}
