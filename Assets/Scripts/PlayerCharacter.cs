using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SandwichInventory))]
public class PlayerCharacter : Character
{
    [SerializeField] public float JumpHeight = 3f;
    [SerializeField] public float HoldGravityAttenuation = 0.4f;

    [SerializeField] public float SandwichForceX = -1000.0f;
    [SerializeField] public float SandwichForceY = 500.0f;

    // Player movement components.
    private BoxCollider2D box;

    // Player movement state.
    private Vector2 position;
    private Vector2 velocity;
    public bool isGrounded { get; private set; }
    private bool isHoldingJump;

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

        // Set up permanent animator values.
        animator.SetFloat("Forward", 1f);
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

    // Update the movement of the player.
    void UpdateMovement()
    {
        if (Input.GetButtonDown("Jump"))
        {
            // Check if we can jump.
            if (isGrounded)
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
        if (!isGrounded)
        {
            animator.SetFloat("Jump", velocity.y);
        }

        // calculate which leg is behind, so as to leave that leg trailing in the jump animation
        // (This code is reliant on the specific run cycle offset in our animations,
        // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
        float runCycleLegOffset = 0f;
        float runCycle =
            Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime + runCycleLegOffset, 1);
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
            }
        }
    }
}
