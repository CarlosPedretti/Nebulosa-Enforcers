using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;
using UnityEngine.UIElements;

public class EnemyHealthSystem : NetworkBehaviour
{
    [SerializeField] private int maxHealth;

    [SerializeField] private int pointsForKilling;

    private int currentHealth;

    private GameObject enemyPrefab;

    [SerializeField] private GameObject explosion_ParticleSys;


    void Start()
    {
        enemyPrefab = GetComponent<EnemyController>().EnemyTypeConfig.EnemyPrefab;
        maxHealth = GetComponent<EnemyController>().EnemyTypeConfig.Health;
        currentHealth = maxHealth;
    }

    public override void OnNetworkSpawn()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, int pID)
    {
        if (IsServer)
        {
            currentHealth -= damage;

            if (currentHealth == 0)
            {
                PlayerLogic[] playerLogics = FindObjectsOfType<PlayerLogic>();

                PlayerLogic playerWithID = System.Array.Find(playerLogics, player => player.playerID == pID);

                if (playerWithID != null)
                {
                    playerWithID.enemysKilled.Value += 1;
                    playerWithID.pointsEarned.Value += pointsForKilling;
                }
				GameObject explosionInstantiated = Instantiate(explosion_ParticleSys, transform.position, transform.rotation);
            explosionInstantiated.GetComponent<NetworkObject>().Spawn();
			
			NetworkObject.Despawn();
            NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, enemyPrefab);
			}            
        }
    }
}