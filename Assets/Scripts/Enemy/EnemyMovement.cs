using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class EnemyMovement : NetworkBehaviour
{
    //[SerializeField] protected BulletConfig bulletConfig;
    [SerializeField] protected float speed;

    protected Rigidbody2D rb;
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

  
    private void FixedUpdate()
    {
        if (!IsServer) return;

        CalculateMovement();
    }

    protected virtual void CalculateMovement()
    {
        Vector2 movement = Vector2.down * speed * Time.fixedDeltaTime;
        movement += rb.position;
        MoveToPosition(movement);
    }
    protected void MoveToPosition(Vector2 position)
    {
        rb.MovePosition(position);
    }




    
}
