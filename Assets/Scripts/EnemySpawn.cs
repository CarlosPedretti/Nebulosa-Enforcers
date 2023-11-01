using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UIElements;

public class EnemySpawn : NetworkBehaviour
{
    [SerializeField] private GameObject enemy;
    public override void OnNetworkSpawn()
    {
        if(!IsServer) return;

        GameObject instansiatedEnemy = Instantiate(enemy);
        instansiatedEnemy.GetComponent<NetworkObject>().Spawn();
    }



}
