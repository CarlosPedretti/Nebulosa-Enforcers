using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class Bullet : NetworkBehaviour
{
    [SerializeField] private float bulletSpeed = 20;

    public override void OnNetworkSpawn()
    {
        GetComponent<Rigidbody2D>().velocity = this.transform.up * bulletSpeed;
    }

}
