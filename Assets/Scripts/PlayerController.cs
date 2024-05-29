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


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        PlayerMovement();
        PlayerJump();
    }

    private void PlayerMovement()
    {
        horizontal = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        anim.SetBool("Walk", horizontal != 0);

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

}
