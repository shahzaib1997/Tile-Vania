using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField] float runSpeed = 5f;
    Rigidbody2D myRigidbody2D;
    CapsuleCollider2D myBodyCollider2D;
    BoxCollider2D myFeet;
    Animator myAnimator;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    float gravityScaleAtStart;
    bool isAlive = true;
    [SerializeField] Vector2 deathKick = new Vector2(0f, 250f);

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
        gravityScaleAtStart = myRigidbody2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive)
        {
            return;
        }
        Run();
        FlipSprite();
        ClimbLadder();
        Jump();
        Die();
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidbody2D.velocity.y);
        myRigidbody2D.velocity = playerVelocity;
        bool playerHasVelocity = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasVelocity);
    }

    private void FlipSprite()
    {
        bool playerHasVelocity = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;
        if (playerHasVelocity)
        {
            transform.localScale = new Vector3(Mathf.Sign(myRigidbody2D.velocity.x), 1f);
        }
    }

    private void ClimbLadder()
    {
        if(!myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myAnimator.SetBool("isClimbing", false);
            myRigidbody2D.gravityScale = gravityScaleAtStart;
            return;
        }
        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidbody2D.velocity.x, controlThrow * climbSpeed);
        myRigidbody2D.velocity = climbVelocity;
        myRigidbody2D.gravityScale = 0f;
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody2D.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    private void Jump()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Foreground")) || myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            return;
        }
        if(CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidbody2D.velocity = myRigidbody2D.velocity + jumpVelocityToAdd;
        }
    }

    private void Die()
    {
        if(myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            myAnimator.SetTrigger("Die");
            myRigidbody2D.velocity = deathKick;
            isAlive = false;
            myRigidbody2D.velocity = new Vector2(0f, myRigidbody2D.velocity.y);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
