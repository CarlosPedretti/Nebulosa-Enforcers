using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BorderController : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer) return;

        if (collision.TryGetComponent(out IProjectile projectile))
        {
            projectile.DespawnProjectile();
        }
    }
}