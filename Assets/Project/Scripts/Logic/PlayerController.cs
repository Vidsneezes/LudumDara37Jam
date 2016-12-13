using UnityEngine;
using System;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float hoverForce;
    public Action onDie;
    private Rigidbody2D rigidBody2d;
    private float addForce;
    private bool alive;

    // Use this for initialization
    private void Start()
    {
        rigidBody2d = GetComponent<Rigidbody2D>();
        addForce = 0;
        alive = true;
        onDie += OnDie;
    }

    private void Update()
    {
        addForce = Input.GetAxis("Fire1");
        if(transform.position.y > 12.5f || transform.position.y < -12.5f)
        {
            Debug.Log("hit");
            if (onDie != null && alive)
            {
                onDie();
            }
        }
    }

    private void FixedUpdate()
    {
        if (alive)
        {
            rigidBody2d.AddForce(Vector2.up * hoverForce * addForce, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider2d)
    {
        if(collider2d.gameObject.CompareTag("Obstacle") && alive)
        {
            Debug.Log("hit");
            if(onDie != null)
            {
                onDie();
            }
        }

        if(collider2d.gameObject.CompareTag("Collectable"))
        {
            CollectableMovingObject cmo = collider2d.GetComponent<CollectableMovingObject>();
            if(cmo.collected == false)// Score additional Points
            {

            }
            cmo.FlyOff();
        }
    }

    private void OnDie()
    {
        alive = false;
        rigidBody2d.AddForce((Vector2.up)* 8, ForceMode2D.Impulse);

        rigidBody2d.AddForce(-(Vector2.right+Vector2.down) * hoverForce *8, ForceMode2D.Impulse);
        rigidBody2d.AddTorque(45,ForceMode2D.Impulse);

        //Run Die Animation
    }
}
