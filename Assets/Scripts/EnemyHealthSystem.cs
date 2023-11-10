using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;

public class EnemyHealthSystem : NetworkBehaviour
{
    [SerializeField] private int maxHealth;

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;

    }

    public void TakeDamage(int damage)
    {
        if(!IsServer) return;

        currentHealth -= damage;

        if (currentHealth < 1)
        {
            Destroy(gameObject);
        }
    }
}