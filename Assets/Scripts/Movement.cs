using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class Movement : NetworkBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float drag = 5f;
    [SerializeField] private float maxTiltAngle = 30f;
    [SerializeField] private float tiltSpeed = 5f;
    private Rigidbody2D rb;

    private Vector2 moveDirection;


    private PlayerInput playerInput;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {

        moveDirection = playerInput.actions["Movement"].ReadValue<Vector2>();
        //float inputY = Input.GetAxis("Vertical");
        //float inputX = Input.GetAxis("Horizontal");

        Move(moveDirection);
        TiltAngle(moveDirection);
    }

    public void Move(Vector2 moveDirection)
    {
        Vector2 movement = new Vector2(speed * moveDirection.x, speed * moveDirection.y);

        rb.AddForce(movement * speed);

        if (rb.velocity.magnitude > speed)
        {
            rb.velocity = rb.velocity.normalized * speed;
        }

        rb.velocity *= (1f - drag * Time.fixedDeltaTime);
    }
    private void TiltAngle(Vector2 moveDirection)
    {
        float tiltAngle = -moveDirection.x * maxTiltAngle;
        Debug.Log(moveDirection.x);
        float currentTilt = Mathf.Lerp(transform.rotation.y, tiltAngle, Time.fixedDeltaTime * tiltSpeed);

        transform.rotation = Quaternion.Euler(0, currentTilt, 0);
    }
}