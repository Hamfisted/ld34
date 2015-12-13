using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    // Component managers.
    protected Animator animator;
    protected CharacterPhysics physics;

    protected void StartCharacter()
    {
        animator = GetComponent<Animator>();
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        physics = new CharacterPhysics(body);
    }

    // Per frame character update.
    protected virtual void UpdateAnimator()
    {
        Vector3 currentVelocity = physics.GetVelocity();
        animator.SetFloat("Forward", Mathf.Abs(currentVelocity.x));
        animator.SetBool("OnGround", physics.isGrounded);
        if (!physics.isGrounded)
        {
            animator.SetFloat("Jump", currentVelocity.y);
        }

        // calculate which leg is behind, so as to leave that leg trailing in the jump animation
        // (This code is reliant on the specific run cycle offset in our animations,
        // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
        float runCycleLegOffset = 0f;
        float runCycle =
            Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime + runCycleLegOffset, 1);
    }

    // Frame update.
    protected void MoveCharacter()
    {
        physics.Move();
    }

    // Handle contact events.
    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            physics.OnContactEvent(contact.normal);
        }
    }

    // Maintain contact so we know when we're continuously on the ground or touching a wall.
    void OnCollisionStay2D(Collision2D collision)
    {
        OnCollisionEnter2D(collision);
    }
}
