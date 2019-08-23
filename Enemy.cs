using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D myRigidbody2D;
    [SerializeField] float moveSpeed = 1f;
    BoxCollider2D myBox;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myBox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (IsFacingRight())
        {
            myRigidbody2D.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            myRigidbody2D.velocity = new Vector2(-moveSpeed, 0f);
        }
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody2D.velocity.x)), 1f);
    }
}
