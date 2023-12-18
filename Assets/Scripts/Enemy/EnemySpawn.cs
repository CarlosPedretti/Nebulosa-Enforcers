using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UIElements;

public class EnemySpawn : NetworkBehaviour
{
    public List<EnemyClass> enemyList;
    [SerializeField] private Transform[] spawns;
    [SerializeField] private float spawnRate;
    [SerializeField] private float timeIncrementRatio = 0.05f;
    [SerializeField] private float minSpawnRate = 0.1f;
    private float nextSpawnTime;
    private int playerCount;

    //private void Start()
    //{
    //    if (IsServer)
    //    {
    //        // Obtener la cantidad de clientes conectados en el servidor
    //        playerCount = NetworkManager.ConnectedClientsList.Count;
    //    }
    //}
    private void Update()
    {
        if (!IsServer) return;
        //playerCount = NetworkManager.ConnectedClientsList.Count;//borrar cuando se junte con el lobby
        //if (Time.time >= nextSpawnTime * (1 / playerCount))

        if (Time.time >= nextSpawnTime)
        {
            int spawnSelected = Random.Range(0, spawns.Length - 1);
            SpawnEnemyServerRPC(spawnSelected);

            spawnRate -= timeIncrementRatio;
            spawnRate = Mathf.Max(spawnRate, minSpawnRate);

            nextSpawnTime = Time.time + spawnRate;
        }
    }

    private GameObject RandomEnemy()
    {
        float totalRatio = 0f;
        foreach (var enemy in enemyList)
        {
            totalRatio += enemy.spawnRatio;
        }

        float randomValue = Random.Range(0f, totalRatio);

        GameObject selectedEnemy = null;
        foreach (var enemy in enemyList)
        {
            if (randomValue <= enemy.spawnRatio)
            {
                selectedEnemy = enemy.enemyPrefab;
                break;
            }
            randomValue -= enemy.spawnRatio;
        }

        return selectedEnemy;
    }


    [ServerRpc]
    private void SpawnEnemyServerRPC(int spawnSelected)
    {
        GameObject enemyToSpawn = RandomEnemy();
        NetworkObject instansiatedEnemy = NetworkObjectPool.Singleton.GetNetworkObject(enemyToSpawn, spawns[spawnSelected].position, enemyToSpawn.transform.rotation);
        instansiatedEnemy.Spawn();
    }
}
[System.Serializable]
public class EnemyClass
{
    public GameObject enemyPrefab;
    public float spawnRatio;
}

