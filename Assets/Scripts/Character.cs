using UnityEngine;
using System.Collections;

public class Character
{
    // Movement parameters.
    [SerializeField] public float Acceleration = 50f;
    [SerializeField] public float JumpSpeed = 10f;
    [SerializeField] public float MaxSideVelocity = 10f;
    [SerializeField] public float AirControl = 0.75f;

    // We want to apply friction manually:
    // 1. Better control when moving on the ground; don't need friction unless player isn't putting in input.
    // 2. Don't want to "rub" down a wall if you're moving into it.
    [SerializeField] public float GroundDeceleration = 15f;
    [SerializeField] public float AirDeceleration = 5f;

    // Movement components.
    public Rigidbody2D body;

    // Movement state.
    private float horizontalVelocity;
    private float wallDirection;
    public bool isGrounded { get; private set; }

    public Character(Rigidbody2D characterBody)
    {
        body = characterBody;
        horizontalVelocity = 0f;
        ResetContacts();
    }

    public void Update(bool applyFriction)
    {
        // Apply friction if we're grounded and had no input.
        if (applyFriction)
        {
            float Deceleration = (isGrounded ? GroundDeceleration : AirDeceleration);
            float DecelerationDirection = (horizontalVelocity > 0f ? -1f : 1f);
            horizontalVelocity += (Deceleration * DecelerationDirection * Time.deltaTime);

            // If velocity is going in same direction as deceleration now, we hit zero.
            if (horizontalVelocity * DecelerationDirection > 0f)
            {
                horizontalVelocity = 0f;
            }
        }
    }

    // Apply acceleration to character's velocity along the XZ plane.
    // Assumes this happens during a non-fixed update and applies delta to acceleration value.
    public void HorizontalAccelerate(float acceleration)
    {
        // Don't allow acceleration into a wall or we can stick on it.
        if (acceleration * wallDirection >= 0f)
        {
            horizontalVelocity += (acceleration * Time.deltaTime);
        }
        ClampVelocity();
    }

    // Launch the character upwards.
    public void Jump(float verticalVelocity)
    {
        Vector2 velocity = body.velocity;
        velocity.y = verticalVelocity;
        body.velocity = velocity;
        isGrounded = false;
    }

    // Override the character's horizontal and vertical velocity.
    public void SetVelocity(Vector2 newVelocity)
    {
        horizontalVelocity = newVelocity.x;
        Jump(newVelocity.y);
        ClampVelocity();
    }

    // Move the character to a given destination, bumping into things along the way.
    public void Move()
    {
        // Reset contacts until next physics update.
        ResetContacts();

        // Apply flat velocity.
        Vector2 velocity = body.velocity;
        velocity.x = horizontalVelocity;
        body.velocity = velocity;
    }

    // Clamp velocity to sane limits.
    public void ClampVelocity()
    {
        horizontalVelocity = Mathf.Clamp(horizontalVelocity, -MaxSideVelocity, MaxSideVelocity);
    }

    // Reset contact events before physics update notifies us of new and persisting contacts.
    void ResetContacts()
    {
        wallDirection = 0f;
        isGrounded = false;
    }

    // Handle collision events for movement.
    public void OnContactEvent(Vector2 normal)
    {
        // Cancel horizontal velocity if we hit a wall.
        float absoluteX = Mathf.Abs(normal.x);
        float absoluteY = Mathf.Abs(normal.y);
        if (absoluteX > absoluteY)
        {
            wallDirection = normal.x;
            horizontalVelocity = 0f;
        }
        else if ((body.velocity.y <= 0f) && (normal.y > 0f))
        {
            isGrounded = true;
        }
    }

}
