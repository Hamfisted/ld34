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
        ProcessInput();
        UpdateController();

        // Flag the controller to apply friction next update unless we bring more horizontal input.
        character.hasFriction = true;
    }

    // Physics update.
    void FixedUpdate()
    {
        MoveController();
    }

    // Process player input.
    void ProcessInput()
    {
        // Side to side movement.
        float horizontalDirection = 0f;
        if (Input.GetButton("Left"))
        {
            horizontalDirection -= 1f;
        }
        if (Input.GetButton("Right"))
        {
            horizontalDirection += 1f;
        }
        horizontalDirection *= character.Acceleration;
        if (!character.isGrounded)
        {
            horizontalDirection *= character.AirControl;
        }
        else if (Input.GetButtonDown("Jump"))
        {
            character.Jump(character.JumpSpeed);
        }

        // Movement notifications set 
        if (Mathf.Abs(horizontalDirection) >= Mathf.Epsilon)
        {
            character.HorizontalAccelerate(horizontalDirection);
        }
    }
}
