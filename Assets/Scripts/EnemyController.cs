using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyController : NetworkBehaviour
{
    [SerializeField] private EnemyConfig enemyTypeConfig;
    private GameObject enemyPrefab;
    public EnemyConfig EnemyTypeConfig { get { return enemyTypeConfig; } }
    private void Awake()
    {
        enemyPrefab = enemyTypeConfig.EnemyPrefab;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer) return;
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<NetworkHealthSystem>().CurrentHealth.Value -= 1;
        }
    }
}
