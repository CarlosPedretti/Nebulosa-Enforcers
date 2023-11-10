using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class EnemyMovement : NetworkBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

  
    private void FixedUpdate()
    {
        if (!IsServer) return;

        Vector2 movement = Vector2.down * speed * Time.fixedDeltaTime;
        movement += rb.position;
        MoveToPosition(movement);
    }

    private void MoveToPosition(Vector2 position)
    {
        rb.MovePosition(position);
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
