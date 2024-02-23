using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 1f;

    public float distance = 1f;
    public bool isVertical = false;
    public Vector2 direction;
    private Vector3 startingPosition;

    
    private void Start()
    {
        direction = isVertical ? Vector2.up : Vector2.right;
        startingPosition = transform.position;
    }
    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, startingPosition + (Vector3)direction * distance, speed * Time.fixedDeltaTime);
        if (Vector3.Distance(startingPosition, transform.position) >= distance)
        {
            direction = -direction;
        }
    }
}
