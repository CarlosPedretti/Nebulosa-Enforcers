using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UIElements;


public class Bullet : NetworkBehaviour
{
    [SerializeField] private BulletConfig bulletConfig;
    [SerializeField] private int damage;
    [SerializeField] private float bulletSpeed = 20;
    [SerializeField] private GameObject bulletPrefab;
    private Rigidbody2D rb;

    private void OnEnable()
    {
        if(bulletConfig != null)
        {
            bulletPrefab = bulletConfig.BulletPrefab;
            bulletSpeed = bulletConfig.Speed;
            damage = bulletConfig.Damage;
        }
    }
    public override void OnNetworkSpawn()
    {
        //if (!IsServer) return;

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * bulletSpeed;

    }
    void Update()
    {
        if (!IsServer) return;

        float screenHeight = Screen.height;
        Debug.Log(Screen.height);
        Vector3 position = transform.position;

        if (position.y > screenHeight || position.y < 0)
        {
            NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, bulletPrefab);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!IsServer) return;
        if(collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyHealthSystem>().TakeDamage(damage);
            NetworkObject.Despawn();

            NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, bulletPrefab);

        }
    }
}
