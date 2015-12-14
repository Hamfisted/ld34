using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerCharacter : Character
{
    [SerializeField] public float JumpHeight = 8f;

    // Player movement components.
    private BoxCollider2D box;

    // Player movement state.
    private Vector2 position;
    private Vector2 velocity;
    private bool isGrounded;

    void Start()
    {
        // Set up components.
        StartCharacter();
        box = GetComponent<BoxCollider2D>();

        // Default state.
        position = body.position;
        velocity = new Vector2();
        isGrounded = false;

        // Set up permanent animator values.
        animator.SetFloat("Forward", 1f);
    }

    // Per-frame controller update.
    void Update()
    {
        UpdateMovement();
        UpdateAnimator();
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
        // Apply gravity.
        velocity += (Physics2D.gravity * Time.deltaTime);

        // Check if we can jump.
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            float JumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * JumpHeight);
            velocity.y = JumpSpeed;
            isGrounded = false;
            return;
        }

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
}
