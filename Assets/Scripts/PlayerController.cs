using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!IsServer) return;
        if(collision.CompareTag("Enemy"))
        {
            GetComponent<NetworkHealthSystem>().CurrentHealth.Value -= 1;
        }
    }
}
  