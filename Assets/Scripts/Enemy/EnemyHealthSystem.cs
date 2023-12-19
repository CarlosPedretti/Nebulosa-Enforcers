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

    [SerializeField] private GameObject enemyPrefab;

    void Start()
    {
        enemyPrefab = GetComponent<EnemyController>().EnemyTypeConfig.EnemyPrefab;
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
            }
            Debug.Log("Modificado desde el serverAAAA");
        }

    }

}