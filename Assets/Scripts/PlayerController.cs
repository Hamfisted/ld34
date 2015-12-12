using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PlayerController : MovementController
{
    private Animator m_Animator;

    void Start()
    {
        Initialize();
        m_Animator = GetComponent<Animator>();
    }

    // Per-frame controller update.
    void Update()
    {
        float horizontalMovement = ProcessHorizontalMovement();
        ProcessHorizontalMovement();
        bool applyFriction = (horizontalMovement == 0f);
        ProcessVerticalMovement();

        character.Update(applyFriction);
        // send input and other state parameters to the animator
        UpdateAnimator(character.body.velocity);
    }

    void UpdateAnimator(Vector2 velocity)
    {
        // update the animator parameters
        m_Animator.SetFloat("Forward", Mathf.Abs(velocity.x), 0.1f, Time.deltaTime);
        // m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        m_Animator.SetBool("OnGround", character.isGrounded);
        if (!character.isGrounded)
        {
            m_Animator.SetFloat("Jump", velocity.y);
        }

        // calculate which leg is behind, so as to leave that leg trailing in the jump animation
        // (This code is reliant on the specific run cycle offset in our animations,
        // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
        float runCycleLegOffset = 0f;
        float runCycle =
            Mathf.Repeat(m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + runCycleLegOffset, 1);
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
