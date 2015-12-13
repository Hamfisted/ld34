using UnityEngine;
using System.Collections;

public class PlayerCharacter: Character
{
    void Start()
    {
        StartCharacter();
    }

    // Per-frame controller update.
    void Update()
    {
        // Handle player specific movement.
        float horizontalMovement = ProcessHorizontalMovement();
        ProcessHorizontalMovement();
        bool applyFriction = (horizontalMovement == 0f);
        ProcessVerticalMovement();
        physics.Update(applyFriction);

        // send input and other state parameters to the animator
        UpdateAnimator();
    }

    // Physics update.
    void FixedUpdate()
    {
        MoveCharacter();
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
        horizontalMovement *= physics.Acceleration;
        if (!physics.isGrounded)
        {
            horizontalMovement *= physics.AirControl;
        }

        // Notify of horizontal acceleration so we don't apply friction.
        if (Mathf.Abs(horizontalMovement) >= Mathf.Epsilon)
        {
            physics.HorizontalAccelerate(horizontalMovement);
            return horizontalMovement;
        }
        return 0f;
    }

    // Get vertical movement.
    void ProcessVerticalMovement()
    {
        if (physics.isGrounded && Input.GetButtonDown("Jump"))
        {
            physics.Jump(physics.JumpSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D triggerCollider)
    {
        Debug.Log("Entered");
    }
}
