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

    private void Update()
    {
        float yBottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).y;

        if (transform.position.y <= yBottom - 10)
        {
            NetworkObject.Despawn();
            NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, enemyPrefab);
        }
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
