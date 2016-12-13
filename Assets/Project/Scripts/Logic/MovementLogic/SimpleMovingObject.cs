using UnityEngine;
using System.Collections;

public class SimpleMovingObject : MonoBehaviour
{

    public GameManager gameManager;
    public float speedMultiplier;
    public Vector2 spawnOffset;
    protected Rigidbody2D rigidBody2d;
    
	protected virtual void Start () {
        rigidBody2d = GetComponent<Rigidbody2D>();
	}
	
    protected virtual void Update()
    {
        if(transform.position.x < -15)
        {
            gameManager.Score(1);
            Destroy(gameObject);
        }
    }

	protected virtual void FixedUpdate () {
        rigidBody2d.MovePosition(rigidBody2d.position - Vector2.right * gameManager.CurrentSpeed * speedMultiplier * Time.deltaTime);
	}

}
