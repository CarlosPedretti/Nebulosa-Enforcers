using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class Bullet : NetworkBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float bulletSpeed = 20;

    public override void OnNetworkSpawn()
    {
        GetComponent<Rigidbody2D>().velocity = this.transform.up * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!IsServer) return;
        if(collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyHealthSystem>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
