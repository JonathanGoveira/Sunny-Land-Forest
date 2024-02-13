using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float slopeCheckDistance;
    [SerializeField] private PhysicsMaterial2D noFrictionMaterial;
    [SerializeField] private PhysicsMaterial2D frictionMaterial;

    private Rigidbody2D playerRB;
    private Animator playerAnimator;
    private CapsuleCollider2D playerCollider;
    private SpriteRenderer playerSpriteRenderer;

    private float moveInput;
    private float slopeAngle;

    private Vector2 perpendicular;
    private Vector2 colliderSize;
    private Vector2 playerPosition;

    private bool facingRight = true;
    private bool isGrounded;
    private bool wasOnGround;
    private bool isJumping;
    private bool isOnSlope;

    // Start is called before the first frame update
    void Start()
    {
        LoadComponents();
    }

    private void LoadComponents()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();

        colliderSize = playerCollider.size;
    }
    // Update is called once per frame
    void Update()
    {
       playerPosition = transform.position - new Vector3(0f, colliderSize.y / 2, 0f);

        DetectGround();
        DetectSlope();
        HandleInput();
        HandleAnimations();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void DetectGround()
    {
        isGrounded = Physics2D.Raycast(playerPosition, Vector3.down, groundCheckDistance, groundMask);

        if (isGrounded && !wasOnGround) HandleLanding();

        wasOnGround = isGrounded;
    }

    private void DetectSlope()
    {
        RaycastHit2D hitSlope = Physics2D.Raycast(playerPosition, Vector2.down, slopeCheckDistance, groundMask);

        if(hitSlope)
        {
            perpendicular = Vector2.Perpendicular(hitSlope.normal).normalized;

            slopeAngle = Vector2.Angle(hitSlope.normal, Vector2.up);
            isOnSlope = slopeAngle != 0;
            print(isOnSlope);
        }

        if(isOnSlope && moveInput == 0)
        {
            playerRB.sharedMaterial = frictionMaterial;
        }
        else
        {
            playerRB.sharedMaterial = noFrictionMaterial;
        }
    }

    private void HandleMovement()
    {
        if (isOnSlope && !isJumping)
        {
            // MOVIMENTAÇÃO NOS SLOPES
            playerRB.velocity = new Vector2(-moveInput * moveSpeed * perpendicular.x, -moveInput * moveSpeed * perpendicular.y);
        }
        else
        {
            // MOVIMENTAÇÃO PADRÃO
            playerRB.velocity = new Vector2(moveInput * moveSpeed, playerRB.velocity.y);
        }
    }

    private void HandleInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            HandleJump();
        }
        if((moveInput > 0 && !facingRight) || (moveInput <0 && facingRight) ) 
        { 
            Flip();
        }
    }

    private void HandleJump()
    {
        isJumping = true;
        playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
    }

    private void HandleLanding()
    {
        isJumping = false;
    }
    private void DebugDraw(RaycastHit2D hitSlope)
    {
        Debug.DrawRay(playerPosition, Vector3.down * groundCheckDistance, Color.blue);
        //Debug.DrawRay(playerPosition, perpendicular, Color.red);
        Debug.DrawRay(playerPosition, hitSlope.normal, Color.green);
        //Debug.DrawRay(playerPosition, Vector2.down * slopeChekDistance, Color.white);
    }

    private void HandleAnimations()
    {
        playerAnimator.SetBool("IsGrounded", isGrounded);
        playerAnimator.SetBool("Walk", Mathf.Abs(moveInput) != 0 && isGrounded);
        playerAnimator.SetBool("Jump", !isGrounded);
    }
    private void Flip()
    {
        facingRight = !facingRight;
        playerSpriteRenderer.flipX = !facingRight;
    }
    
}
