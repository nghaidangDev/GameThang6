using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    private bool isFacingRight;
    private float horizontal;

    private Rigidbody2D rb;
    private Animator anim;


    [Header("Jump")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    [Header("Wall")]
    [SerializeField] private float wallCheckDistance = 0.4f;
    [SerializeField] private Transform wallCheck;
    private bool isWallSliding;
    private float wallSlideSpeed;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckIsWallSliding();
        PlayerMovement();
        PlayerJump();

    }

    private void PlayerMovement()
    {
        horizontal = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        anim.SetBool("Walk", horizontal != 0);

        //Wall Sliding
        if (isWallSliding)
        {
            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
        anim.SetBool("Sliding", isWallSliding);

        Flip();
    }

    private void PlayerJump()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetTrigger("Jump");
        }
    }

    private void CheckIsWallSliding()
    {
        if (isWalled() && !isGrounded() && rb.velocity.y < 0)
        {
            Debug.Log("Yes");
            isWallSliding = true;
        }
        else
        {
            Debug.Log("NO");
            isWallSliding = false;
        }
    }

    private void Flip()
    {
        if ((isFacingRight && horizontal > 0) || (!isFacingRight && horizontal < 0) )
        { 
            isFacingRight = !isFacingRight;
            Vector2 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool isWalled()
    {
        return Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, groundLayer);
    }

}
