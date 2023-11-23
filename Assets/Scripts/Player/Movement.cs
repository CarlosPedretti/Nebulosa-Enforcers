using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class Movement : NetworkBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float drag = 5f;
    [SerializeField] private Rigidbody2D rb;

    private Vector2 moveDirection;


    private PlayerInput playerInput;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void FixedUpdate()
    {

        Move(moveDirection);

    }

    public void Move(Vector2 moveDirection)
    {
        moveDirection = playerInput.actions["Movement"].ReadValue<Vector2>();

        Vector2 movement = new Vector2(speed * moveDirection.x, speed * moveDirection.y);

        rb.AddForce(movement * speed);

        if (rb.velocity.magnitude > speed)
        {
            rb.velocity = rb.velocity.normalized * speed;
        }

        rb.velocity *= (1f - drag * Time.fixedDeltaTime);
    }
}