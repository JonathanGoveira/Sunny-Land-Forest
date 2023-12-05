using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{
    // propriedades do player
    private Animator playerAnimator;
    private Rigidbody2D playerRigidbody2D;
    private SpriteRenderer playerSpriteRenderer;
    private bool playerIvuneravel;
    public GameObject playerDie;
    // filho do player que verifica se o player está tocando no chão
    [Header("Collision Settings")]
    public bool isGround;
    private Transform groundCheck;

    // variaveis de movimentação
    [Header("Move Settings")]
    public float speed;
    public float touchRun = 0.0f;
    private bool facinRight = true;

    // pulo
    [Header("Jump Settings")]
    public int numberJumps;
    public int maxJump;
    public float jumpForce;
    public float jumpEnemieForce;
    private bool jump = false;

    // game control
    [Header("Audio Settings")]
    public AudioSource fxGame;
    public AudioClip fxPulo;
    public int vidas = 3;
    public Color hitColor;
    private GameControl _gameControl;

    // Start is called before the first frame update
    void Start()
    {
        //groundCheck = this.gameObject.transform.GetChild(0);
        groundCheck = GameObject.Find("GroundCheck").transform;

        playerAnimator = GetComponent<Animator>();
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();

        //_gameControl = FindObjectOfType(typeof(GameControl)) as GameControl;
        _gameControl = FindObjectOfType<GameControl>();

    }

    // Update is called once per frame
    void Update()
    {
        isGround = CheckCollisionLinecast(this.transform,groundCheck);
        SetMotions();

        SetAnimations();
        
    }

    private void FixedUpdate()
    {
        MovePlayer(touchRun);

        if (jump)
        {
            JumpPlayer();
        }
    }

    void MovePlayer(float movimentoH) 
    {
        playerRigidbody2D.velocity = new Vector2(movimentoH * speed,playerRigidbody2D.velocity.y);
        
        if(movimentoH < 0 && facinRight || (movimentoH > 0 && !facinRight)) 
        {
            Flip();
        }
    }

    void Flip()
    {
        facinRight = !facinRight;
        // inverte o sinal do transform para fazer o flip do personagem, 1 passa a ser -1 e vice e versa
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void SetMotions() {
        touchRun = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }
    void SetAnimations()
    {
        playerAnimator.SetBool("IsGrounded", isGround);
        playerAnimator.SetBool("Walk", playerRigidbody2D.velocity.x != 0 && isGround); //true ou false dependo se tiver movimento no eixo x
        playerAnimator.SetBool("Jump", !isGround);
    }

    void JumpPlayer() 
    {
        if (isGround) 
        { 
            numberJumps = 0;
        }

        if (isGround || numberJumps < maxJump) 
        {
            playerRigidbody2D.AddForce(new Vector2(0f, jumpForce));
            isGround = false;
            numberJumps++;
            fxGame.PlayOneShot(fxPulo);
        }
        jump = false;
    }

    bool CheckCollisionLinecast(Transform obj1, Transform obj2) 
    {
        // traça uma linha entre o player e seu objeto filho groundCheck e caso tenha um colisor com a layer "Ground" entre essa linha retorna true
        bool collided;
        collided = Physics2D.Linecast(obj1.position, obj2.position, 1 << LayerMask.NameToLayer("Ground"));
        //isGround = collided;
        return collided;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Coletaveis":
                _gameControl.Pontuacao(1);
                Destroy(collision.gameObject);
                break;

            case "Inimigos":

                GameObject explosao = Instantiate(_gameControl.hitInimigoMortoPrefab, this.transform.position, this.transform.localRotation);
                Destroy(explosao, 0.5f);

                Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
                rigidbody.AddForce(new Vector2(0, jumpEnemieForce));

                _gameControl.fxGame.PlayOneShot(_gameControl.fxInimigoMorto);

                Destroy(collision.gameObject);
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Inimigos":
                Hurt();
                
                break;
        }
    }

    void Hurt()
    {
        if (!playerIvuneravel)
        { 
            playerIvuneravel = true;
            vidas--;
            StartCoroutine("Dano");
           _gameControl.BarraDeVidas(vidas);

            if (vidas < 1)
            {
                GameObject pDieTemp = Instantiate(playerDie, this.transform.position, Quaternion.identity);
                Rigidbody2D rbDie = pDieTemp.GetComponent<Rigidbody2D>();
                rbDie.AddForce(new Vector2(150f, 500f));
                _gameControl.fxGame.PlayOneShot(_gameControl.fxDie);
                gameObject.SetActive(false);
            }
        
        }

    }
    IEnumerator Dano()
    {
        playerSpriteRenderer.color = hitColor;
        yield return new WaitForSeconds(0.1f);

        for (float i = 0; i<1; i += 0.1f)
        {
            playerSpriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            playerSpriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        playerSpriteRenderer.color = Color.white;
        playerIvuneravel = false;
    }
}
