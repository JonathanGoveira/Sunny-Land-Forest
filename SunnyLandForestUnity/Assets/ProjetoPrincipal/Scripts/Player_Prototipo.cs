using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player_Prototipo : MonoBehaviour
{

    [SerializeField] private float moveSpeed;
    [SerializeField] private float JumpForce;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float slopeChekDistance;
    [SerializeField] private PhysicsMaterial2D noFrictionMaterial;
    [SerializeField] private PhysicsMaterial2D frictionMaterial;

    private Rigidbody2D playerRB;
    private Animator playerAnimator;
    private CapsuleCollider2D playerCollider;
    private SpriteRenderer playerSpriteRenderer;

    private float moveInput;

    private Vector2 perpendicularSpeed;
    private Vector2 colliderSize;
    private Vector2 position;
    private Vector2 forcedDirection = new Vector2(0,0);

    private bool facingRight = true;
    private bool isGrounded;


    // Start is called before the first frame update
    void Start()
    {
        LoadComponents();
    }

    // Update is called once per frame
    void Update()
    {
        position = transform.position - new Vector3(0f, colliderSize.y / 2, 0f);

        HandleInput();
        HandleAnimations();
    }

    private void FixedUpdate()
    {
        MoveAndNormalizeSlopes();
    }
    private void LoadComponents()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();

        colliderSize = playerCollider.size;
    }

    private void DebugDraw(RaycastHit2D hitSlope)
    {
        Debug.DrawRay(position, Vector3.down * groundCheckDistance, Color.blue);
        Debug.DrawRay(position, forcedDirection, Color.red);
        Debug.DrawRay(position, perpendicularSpeed, Color.red);
        Debug.DrawRay(position, hitSlope.normal, Color.green);
        Debug.DrawRay(position, Vector2.down * slopeChekDistance, Color.white);
    }

    private void MoveAndNormalizeSlopes()
    {
        RaycastHit2D hitDirection = Physics2D.Raycast(transform.position, Vector2.down, slopeChekDistance, groundMask);

        if(hitDirection.collider != null)
        {
            isGrounded = true;

            if(isGrounded && moveInput >= 0)
            {
                forcedDirection = new Vector2(hitDirection.normal.y, -hitDirection.normal.x);

            }
            else
            {
                forcedDirection = new Vector2(-hitDirection.normal.y, hitDirection.normal.x);
            }
            //transform.Translate(forcedDirection * Mathf.Abs(moveInput) * moveSpeed);
            playerRB.velocity = new Vector2(forcedDirection.x * moveSpeed, playerRB.velocity.y);
        }
        else { isGrounded = false; }
        playerRB.velocity = new Vector2(moveInput * moveSpeed, playerRB.velocity.y);
        Debug.Log(isGrounded);
        DebugDraw(hitDirection);
    }

    private void HandleInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {   
            HandleJump();
        }
        if ((moveInput > 0 && !facingRight) || (moveInput < 0 && facingRight))
        {
            Flip();

        }
    }

    private void HandleAnimations()
    {
        playerAnimator.SetBool("IsGrounded", isGrounded);
        playerAnimator.SetBool("Walk", Mathf.Abs(moveInput) != 0 && isGrounded);
        playerAnimator.SetBool("Jump", !isGrounded);
    }
    private void HandleJump()
    {
        playerRB.velocity = new Vector2(playerRB.velocity.x * JumpForce, JumpForce);
        //playerRB.AddForce(new Vector2(0f, JumpForce));
    }
    private void Flip()
    {
        facingRight = !facingRight;
        playerSpriteRenderer.flipX = !facingRight;
    }

}
