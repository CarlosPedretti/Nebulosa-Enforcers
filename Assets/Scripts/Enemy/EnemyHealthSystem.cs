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

    [SerializeField] private GameObject enemyPrefab;

    void Start()
    {
        enemyPrefab = GetComponent<EnemyController>().EnemyTypeConfig.EnemyPrefab;
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
            NetworkObjectPooll.Singleton.ReturnNetworkObject(NetworkObject, enemyPrefab);
        }
    }
}