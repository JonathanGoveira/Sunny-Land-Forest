using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Player_Controller : MonoBehaviour
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
    private float slopeAngle;

    private Vector2 colliderSize;
    private Vector2 position;

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

    // Update is called once per frame
    void Update()
    {
        position = transform.position - new Vector3(0f, colliderSize.y / 2, 0f);

        DetectGround();
        DetectSlopes();
        HandleInput();
        HandleAnimations();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
    private void LoadComponents() 
    { 
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();

        colliderSize = playerCollider.size;
    }
    private void DetectGround () 
    {
        isGrounded = Physics2D.Raycast(position, Vector3.down, groundCheckDistance, groundMask);
        if (isGrounded && !wasOnGround) HandleLanding();
        wasOnGround = isGrounded;
    }

    private void DetectSlopes()
    {
        RaycastHit2D hitSlopes = Physics2D.Raycast(position, Vector2.down, slopeChekDistance, groundMask);
        if(hitSlopes)
        {
            slopeAngle = Vector2.Angle(hitSlopes.normal, Vector2.up);
            isOnSlope = slopeAngle != 0;
            
        }
        if (isOnSlope && moveInput == 0)
        {
            playerRB.sharedMaterial = frictionMaterial;
        }
        else
        {
            playerRB.sharedMaterial = noFrictionMaterial;
        }
    }

    private void HandleInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded) 
        { 
            HandleJump();
        }
        if((moveInput > 0 && !facingRight) || (moveInput < 0 && facingRight)) 
        {
            Flip();
        
        }
    }

    private void HandleAnimations()
    {
        playerAnimator.SetBool("IsGrounded", isGrounded);
        playerAnimator.SetBool("Walk", Mathf.Abs(moveInput) != 0 && isGrounded);
        //playerAnimator.SetBool("Walk", playerRB.velocity.x != 0 && isGrounded); //true ou false dependo se tiver movimento no eixo x (BUG DA RAMPA AQUI)
        playerAnimator.SetBool("Jump", !isGrounded);
        Debug.Log(playerRB.velocity.x);
    }

    private void HandleLanding() 
    {
        isJumping = false;
    }

    private void HandleJump()
    {
        isJumping = true;
        playerRB.velocity = new Vector2(playerRB.velocity.x, JumpForce);
    
    }

    private void HandleMovement()
    {
        playerRB.velocity = new Vector2(moveInput * moveSpeed, playerRB.velocity.y);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        //transform.Rotate(0f, 180f, 0f);
        //transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        playerSpriteRenderer.flipX = !facingRight;
    }
    //https://www.youtube.com/watch?v=NweTWCYjxac&t=801s
}
