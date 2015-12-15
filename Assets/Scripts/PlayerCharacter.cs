using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SandwichInventory))]
public class PlayerCharacter : Character
{
    [SerializeField] public float JumpHeight = 2f;
    [SerializeField] public float HoldGravityAttenuation = 0.25f;

    [SerializeField] public float SandwichForceX = -1000.0f;
    [SerializeField] public float SandwichForceY = 500.0f;

    // Player movement components.
    private BoxCollider2D box;

    // Player movement state.
    private Vector2 position;
    private Vector2 velocity;
    public bool isGrounded { get; private set; }
    private bool isHoldingJump;
    public bool offPlatform { get; private set; }
    public bool isDead = false;
    public bool hasWon = false;
    public int health;

    void Start()
    {
        // Set up components.
        StartCharacter();
        box = GetComponent<BoxCollider2D>();

        // Default state.
        position = body.position;
        velocity = new Vector2();
        isGrounded = false;
        isHoldingJump = false;
        offPlatform = false;
        isDead = false;
        hasWon = false;
        health = 3;
    }

    // Per-frame controller update.
    void Update()
    {
        UpdateMovement();
        UpdateAnimator();
        UpdateActions();
    }

    // Physics update.
    void FixedUpdate()
    {
        // Apply movement.
        body.MovePosition(position);
    }

    // Set up for next physics update.
    void LateUpdate()
    {
    }

    public void TakeDamage()
    {
        health--;
        if (health == 0)
        {
            isDead = true;
        }
    }

    // Update the movement of the player.
    void UpdateMovement()
    {
        if (Input.GetButtonDown("Jump"))
        {
            // Check if we can jump.
            if (isGrounded && !hasWon && !isDead)
            {
                float JumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * JumpHeight);
                velocity.y = JumpSpeed;
                isGrounded = false;
                isHoldingJump = true;
                return;
            }
        }
        else if (!Input.GetButton("Jump"))
        {
            isHoldingJump = false;
        }

        // Apply gravity.
        float attenuation = (isHoldingJump ? HoldGravityAttenuation : 1f);
        velocity += (Physics2D.gravity * Time.deltaTime * attenuation);

        // Check if we have floor below us if we're falling.
        offPlatform = false;
        Vector2 velocityDelta = velocity * Time.deltaTime;
        Vector2 newPosition = position + velocityDelta;
        if (velocityDelta.y < 0f)
        {
            // Get the platforms we're possibly touching.
            Vector2 extents = box.size * 0.5f;
            Vector2 min = position - extents + velocityDelta;
            Vector2 max = position + extents;
            Collider2D[] colliders = Physics2D.OverlapAreaAll(min, max);
            
            // Look for a hit on a platform.
            foreach (Collider2D collider in colliders)
            {
                float maxY = collider.bounds.max.y;
                if ((position.y >= maxY) && (newPosition.y < maxY))
                {
                    if (collider.CompareTag("Floor"))
                    {
                        offPlatform = true;
                    }

                    if (collider.CompareTag("Floor") || collider.CompareTag("Platform"))
                    {
                        position.y = maxY;
                        velocity.y = 0f;
                        isGrounded = true;
                        return;
                    }
                  
                }
            }
        }
        isGrounded = false;
        position = newPosition;
    }

    // Per frame character update.
    protected virtual void UpdateAnimator()
    {
        animator.SetBool("OnGround", isGrounded);
        animator.SetBool("HasWon", hasWon);
        animator.SetBool("IsDead", isDead);
    }

    void UpdateActions()
    {
        if (Input.GetButtonDown("ThrowSandwich"))
        {
            var sandy = gameObject.GetComponent<SandwichInventory>().UseSandwich();
            if (sandy)
            {
                var trans = sandy.GetComponent<Transform>();
                trans.position = gameObject.transform.position;

                var body = sandy.GetComponent<Rigidbody2D>();
                body.AddForce(new Vector2(SandwichForceX, SandwichForceY));

                animator.SetTrigger("Throw");
                animator.SetLayerWeight(1, 1);
            }
        }

        if (Input.GetButtonDown("Restart"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetButtonDown("Quit"))
        {
            Debug.Log("HELLO");
            Application.Quit();
        }
    }

}
