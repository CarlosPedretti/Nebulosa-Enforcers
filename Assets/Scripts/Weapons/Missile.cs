using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Missile : NetworkBehaviour, IProjectile
{
    [SerializeField] private BulletConfig bulletConfig;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotacionSpeed = 200f;
    [SerializeField] private float explosionRadius = 5;
    [SerializeField] private int damage = 2;
    private GameObject bulletPrefab;
    public NetworkVariable<int> currentPlayerBullet = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    private Transform target;
    private Rigidbody2D rb;
    public Transform Target { set { target = value; } }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        if (bulletConfig != null)
        {
            bulletPrefab = bulletConfig.BulletPrefab;
            speed = bulletConfig.Speed;
            damage = bulletConfig.Damage;
        }
    }

    void FixedUpdate()
    {
        if (!IsServer) return;

        MoveToTarget();
    }
    private void MoveToTarget()
    {
        if (target != null)
        {
            Vector2 direccion = (Vector2)target.position - rb.position;
            direccion.Normalize();

            float rotacion = Vector3.Cross(direccion, transform.up).z;
            rb.angularVelocity = -rotacion * rotacionSpeed;

            rb.velocity = transform.up * speed;
        }
        else
        {
            rb.velocity = transform.up * speed;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return;

        Collider2D[] collisionObj = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D objeto in collisionObj)
        {

            if (objeto.gameObject.TryGetComponent(out EnemyHealthSystem enemyHealthSystem))
            {
                enemyHealthSystem.TakeDamage(damage, currentPlayerBullet.Value);
            }
        }
        DespawnProjectile();
    }
    public void DespawnProjectile()
    {
        if (!IsServer) return;

        NetworkObject.Despawn();
        NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, bulletPrefab);

    }
}