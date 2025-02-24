using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Brandon Callaway | 000954560
public class PlayerController : MonoBehaviour
{
    // Component Reference Variables
    [Header("References")]
    public Rigidbody2D rb2d;
    public GameObject respawnPoint;
    public Transform feet;
    public Animator anim;
    public SpriteRenderer sr;
    public BoxCollider2D interactArea;

    // Heart Container Variables
    [Header("Hearts")]
    public GameObject[] heartContainers = new GameObject[3];
    public GameObject[] heartBackgrounds = new GameObject[3];
    public int totalHearts = 3;
    private float remainingHearts;
    private bool updateHearts = false;

    // Player Movement Variables
    [Header("Movement")]
    public float moveSpeed = 8f;
    public bool canPlayerMove = true;

    // Jumping Variables
    [Header("Jumping")]
    public LayerMask groundLayer;
    public bool isGrounded;
    public float jumpHeight = 10;
    public float fallMultiplier = 5;
    public float lowJumpMultiplier = 2.5f;
    public float coyoteTimeMax = 0.1f;
    private float coyoteTime;

    // Interaction Variables
    [Header("Interaction")]
    public bool isInteracting = false;
    public float interactTimer;
    public float interactTimerMax = 0.3f;

    // Attack Variables
    [Header("Attacking")]
    public CircleCollider2D attackCollider;
    public bool isAttacking = false;
    public float attackTimerMax = 0.3f;
    private float attackTimer;

    // Player Damaged Variables
    [Header("Player Damage")]
    public bool hasTakenDamage = false;
    public bool hasDied = false;
    public float damageTimerMax = 0.5f;
    private float damageTimer = 0f;

    // Miscellanous Variables
    [Header("Misc.")]
    public int playerCoins = 0;
    public bool hasKey = false;

    // Powerup Variables
    public bool jumpBoost = false;


    // Start is called before the first frame update
    void Start()
    {
        //Inirialize the rb2d variable
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        interactArea = GetComponentInChildren<BoxCollider2D>();
        interactArea.enabled = false;
        attackCollider = GetComponentInChildren<CircleCollider2D>();
        attackCollider.enabled = false;

        totalHearts = heartContainers.Length;
        remainingHearts = totalHearts;
        coyoteTime = coyoteTimeMax;
        interactTimer = interactTimerMax;
        attackTimer = attackTimerMax;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");

        isGrounded = Physics2D.Raycast(feet.position, Vector2.down, 1f, groundLayer);

        if (jumpBoost)
        {
            jumpHeight = 18f;
        }

        Movement(h);
        HandleJump();
        HandleAttacks();
        HandleInteractions();
        HandleDamage();
    }

    void HandleInteractions()
    {
        // Disable interaction if interactTimer reaches 0
        if (interactTimer < 0)
        {
            isInteracting = false;
        }

        // If the player is interacting, stop the players velocity and ability to move, and enable the interact collider
        // Alongside this, set the player sprite to the interact animation
        if (isInteracting)
        {
            canPlayerMove = false;
            interactArea.enabled = true;
            rb2d.velocity = Vector2.zero;
            interactTimer -= Time.deltaTime;
            anim.SetBool("isInteracting", true);
        }
        else
        {
            canPlayerMove = true;
            interactArea.enabled = false;
            interactTimer = attackTimerMax;
            anim.SetBool("isInteracting", false);
        }

        if (Input.GetKeyDown(KeyCode.E) && isGrounded)
        {
            isInteracting = true;
            interactTimer = interactTimerMax;
        }
    }

    void HandleAttacks()
    {
        // Offset attack collider, based on which way the player sprite is facing
        if (sr.flipX == true)
        {
            attackCollider.offset = new Vector2(-1f, 0f);
        }
        else
        {
            attackCollider.offset = new Vector2(0f, 0f);
        }

        // Disable attack if attackTimer reaches 0
        if (attackTimer < 0)
        {
            isAttacking = false;
        }

        // If the player is attacking, stop the players velocity and ability to move, and enable the attack collider
        // Alongside this, set the player sprite to punch animation
        if (isAttacking)
        {
            canPlayerMove = false;
            attackCollider.enabled = true;
            rb2d.velocity = Vector2.zero;
            attackTimer -= Time.deltaTime;
            anim.SetBool("isAttacking", true);
        }
        else
        {
            canPlayerMove = true;
            attackCollider.enabled = false;
            attackTimer = attackTimerMax;
            anim.SetBool("isAttacking", false);
        }

        // Attack if the player presses the attack key, and is grounded
        if (Input.GetKeyDown(KeyCode.LeftAlt) && isGrounded)
        {
            isAttacking = true;
            damageTimer = damageTimerMax;
        }
    }

    void Movement(float hSpeed)
    {
        if (canPlayerMove)
        {
            rb2d.velocity = new Vector2(hSpeed * moveSpeed, rb2d.velocity.y);
            anim.SetFloat("walkspeed", Mathf.Abs(hSpeed));
            if (hSpeed < 0)
            {
                sr.flipX = true;
            }
            else if (hSpeed > 0)
            {
                sr.flipX = false;
            }
        }
    }

    void HandleDamage()
    {
        // Handle damage animation and logic
        if (hasTakenDamage)
        {
            canPlayerMove = false;
            rb2d.velocity = Vector2.zero;
            damageTimer -= Time.deltaTime;
            anim.SetBool("playerDamaged", true);

            if (damageTimer < 0)
            {
                remainingHearts--;
                updateHearts = true;
                rb2d.MovePosition(respawnPoint.transform.position);
                hasTakenDamage = false;
            }
        }
        else
        {
            canPlayerMove = true;
            damageTimer = damageTimerMax;
            anim.SetBool("playerDamaged", false);
        }

        if (hasDied)
        {
            SceneManager.LoadScene(3);
        }

        // Handle heart containers
        if (updateHearts)
        {
            if (remainingHearts == 3)
            {
                heartContainers[0].SetActive(true);
                heartContainers[1].SetActive(true);
                heartContainers[2].SetActive(true);
            }
            else if (remainingHearts == 2)
            {
                heartContainers[0].SetActive(true);
                heartContainers[1].SetActive(true);
                heartContainers[2].SetActive(false);
            }
            else if (remainingHearts == 1)
            {
                heartContainers[0].SetActive(true);
                heartContainers[1].SetActive(false);
                heartContainers[2].SetActive(false);
            }
            else if (remainingHearts == 0)
            {
                heartContainers[0].SetActive(false);
                heartContainers[1].SetActive(false);
                heartContainers[2].SetActive(false);
                rb2d.velocity = Vector2.zero;
                canPlayerMove = false;
                hasDied = true;
            }
            updateHearts = false;
        }
    }
    void HandleJump()
    {
        if (coyoteTime > 0 && Input.GetKeyDown(KeyCode.Space))
        { 
           rb2d.velocity = new Vector2(transform.position.x, jumpHeight);
        }

        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += (Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
        }

        else if (rb2d.velocity.y > 0 && !Input.GetKey(KeyCode.P))
        {
            rb2d.velocity += (Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime);
        }

        if (isGrounded)
        {
            coyoteTime = coyoteTimeMax;
        }
        else
        {
            coyoteTime -= Time.deltaTime;
        }

        anim.SetBool("isJumping", !isGrounded);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Collectable")
        {
            playerCoins++;
            Debug.Log($"Total Player Coins: {playerCoins}");
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Spikes")
        {
            damageTimer = damageTimerMax;
            hasTakenDamage = true;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player enters the killzone box at the base of the map, reset the players position to the spawn point
        if (collision.gameObject.tag == "Killzone")
        {
            damageTimer = damageTimerMax;
            hasTakenDamage = true;
        }

        // If player collides with the key, disable key object, and toggle key bool
        if (collision.gameObject.tag == "Key")
        {
            hasKey = true;
            collision.gameObject.SetActive(false);
        }

        // If player collides with the exit door, and has a key, load win scene
        if (collision.gameObject.tag == "door" && hasKey)
        {
            SceneManager.LoadScene(1);
        }

        if (collision.gameObject.tag == "jumpBoost")
        {
            jumpBoost = true;
            GameObject.Destroy(collision.gameObject);
        }
    }
}
