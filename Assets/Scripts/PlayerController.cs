using UnityEngine;
using System.Collections;

public class PlayerController : MovementController
{
    void Start()
    {
        Initialize();
    }

    // Per-frame controller update.
    void Update()
    {
        float horizontalMovement = ProcessHorizontalMovement();
        ProcessHorizontalMovement();
        bool applyFriction = (horizontalMovement == 0f);
        ProcessVerticalMovement();

        character.Update(applyFriction);
    }

    // Physics update.
    void FixedUpdate()
    {
        character.Move();
    }

    // Set up for next physics update.
    void LateUpdate()
    {
    }

    // Process player input.
    // Returns the effective acceleration applied.
    float ProcessHorizontalMovement()
    {
        // Side to side movement.
        float horizontalMovement = 0f;
        if (Input.GetButton("Left"))
        {
            horizontalMovement -= 1f;
        }
        if (Input.GetButton("Right"))
        {
            horizontalMovement += 1f;
        }
        horizontalMovement *= character.Acceleration;
        if (!character.isGrounded)
        {
            horizontalMovement *= character.AirControl;
        }

        // Notify of horizontal acceleration so we don't apply friction.
        if (Mathf.Abs(horizontalMovement) >= Mathf.Epsilon)
        {
            character.HorizontalAccelerate(horizontalMovement);
            return horizontalMovement;
        }
        return 0f;
    }

    // Get vertical movement.
    void ProcessVerticalMovement()
    {
        if (character.isGrounded && Input.GetButtonDown("Jump"))
        {
            character.Jump(character.JumpSpeed);
        }
    }
}
