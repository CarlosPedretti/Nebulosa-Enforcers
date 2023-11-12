using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;
using UnityEngine.UIElements;

public class EnemyHealthSystem : NetworkBehaviour
{
    [SerializeField] private int maxHealth;

    private int currentHealth;

    private GameObject enemyPrefab;

    void Start()
    {
        enemyPrefab =gameObject.GetComponent<EnemyController>().EnemyTypeConfig.EnemyPrefab;
    }
    public override void OnNetworkSpawn()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!IsServer) return;

        currentHealth -= damage;

        if (currentHealth < 1)
        {
            //Destroy(gameObject);
            NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, enemyPrefab);
        }
    }
}