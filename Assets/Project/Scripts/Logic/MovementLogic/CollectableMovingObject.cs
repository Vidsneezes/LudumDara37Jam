using UnityEngine;
using System.Collections;

public class CollectableMovingObject : SimpleMovingObject {

    public bool collected;

    protected override void Start()
    {
        collected = false;
        rigidBody2d = GetComponent<Rigidbody2D>();
    }

    protected override void FixedUpdate()
    {
        if (collected == false)
        {
            rigidBody2d.MovePosition(rigidBody2d.position - Vector2.right * gameManager.CurrentSpeed * speedMultiplier * Time.deltaTime);
        }
    }

    public void FlyOff()
    {
        if (collected == false)
        {
            collected = true;
            gameManager.Score(4);
            rigidBody2d.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
            StartCoroutine(RemoveAfter());
        }
    }

    private IEnumerator RemoveAfter()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

}
