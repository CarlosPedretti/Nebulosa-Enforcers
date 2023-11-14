using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementSenoidal : EnemyMovement
{


    [Header("Wavy Variables")]
    [SerializeField] private float amplitude;
    [SerializeField] private float period;
    [SerializeField] private float shift;
    [SerializeField] private float yChange;
    private float newX;
    private float newY;



    protected override void CalculateMovement()
    {
        Vector2 movementDirection;

        movementDirection.y = -speed * Time.fixedDeltaTime;
        movementDirection.x = amplitude * Mathf.Sin(period * movementDirection.y) + shift;

        movementDirection += rb.position;
        MoveToPosition(movementDirection);
    }    
}