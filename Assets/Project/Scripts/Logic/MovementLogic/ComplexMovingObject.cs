using UnityEngine;
using System.Collections;

public class ComplexMovingObject : SimpleMovingObject {

    public float verticalSpeed;
    public float limits; 
    private Vector3 spawnPosition;
    private Vector2 velocity;

    protected override void Start()
    {
        rigidBody2d = GetComponent<Rigidbody2D>();
        spawnPosition = transform.position;
        velocity = Vector2.zero;
        velocity.x = -(Vector2.right * gameManager.CurrentSpeed * speedMultiplier).x;
        velocity.y = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
        velocity.y *= verticalSpeed;
    }

    protected override void FixedUpdate()
    {
        if (rigidBody2d.position.y > spawnPosition.y + limits || rigidBody2d.position.y < spawnPosition.y - limits)
        {
            velocity.y *= -1;
        }
        rigidBody2d.MovePosition(rigidBody2d.position + velocity * Time.deltaTime);

    }
}
