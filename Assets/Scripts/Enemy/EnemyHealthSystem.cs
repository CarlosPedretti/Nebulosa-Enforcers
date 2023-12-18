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
    [SerializeField] private GameObject explosion_ParticleSys;

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
            GameObject explosionInstantiated = Instantiate(explosion_ParticleSys, transform.position, transform.rotation);
            explosionInstantiated.GetComponent<NetworkObject>().Spawn();

            NetworkObject.Despawn();
            NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, enemyPrefab);
        }
    }
}