using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Movement : NetworkBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float drag = 5f;
    [SerializeField] private Rigidbody2D rb;


    void FixedUpdate()
    {

        float inputY = Input.GetAxis("Vertical");
        float inputX = Input.GetAxis("Horizontal");

        Move(inputX, inputY);

    }


    private void Move(float inputX, float inputY)
    {
        Vector2 movement = new Vector2(speed * inputX, speed * inputY);

        rb.AddForce(movement * speed);


        if (rb.velocity.magnitude > speed)
        {
            rb.velocity = rb.velocity.normalized * speed;
        }

        rb.velocity *= (1f - drag * Time.fixedDeltaTime);
    }
}