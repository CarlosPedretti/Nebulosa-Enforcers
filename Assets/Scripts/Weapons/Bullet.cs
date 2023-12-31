using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UIElements;
using System;


public class Bullet : NetworkBehaviour, IProjectile
{
    [SerializeField] private BulletConfig bulletConfig;
    [SerializeField] private int damage;
    [SerializeField] private float bulletSpeed = 20;

    [SerializeField] private GameObject bulletPrefab;

    public NetworkVariable<int> currentPlayerBullet = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private Rigidbody2D rb;

    private void OnEnable()
    {
        if (bulletConfig != null)
        {
            bulletPrefab = bulletConfig.BulletPrefab;
            bulletSpeed = bulletConfig.Speed;
            damage = bulletConfig.Damage;
        }
    }
    public override void OnNetworkSpawn()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * bulletSpeed;
    } 

    public void DespawnProjectile()
    {
        if (!IsServer) return;

        NetworkObject.Despawn();
        NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, bulletPrefab);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!IsServer) return;
        if(collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyHealthSystem>().TakeDamage(damage, currentPlayerBullet.Value);
            DespawnProjectile();     
        }
    }

    public void GetShooterId(int pID)
    {
        currentPlayerBullet.Value = pID;
    }
}