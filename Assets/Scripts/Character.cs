using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    // Movement parameters.
    [SerializeField] public float Acceleration = 50f;
    [SerializeField] public float AirControl = 0.75f;

    // Component managers.
    protected Animator animator;
    protected CharacterPhysics physics;

    // Navigation nodes.
    public HashSet<PathNode> nodes { get; protected set; }
    public PathNode latestNode { get; protected set; }

    protected void StartCharacter()
    {
        animator = GetComponent<Animator>();
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        physics = new CharacterPhysics(body);
        nodes = new HashSet<PathNode>();
        latestNode = null;
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

    // Handle trigger enter events to get which node we're in.
    void OnTriggerEnter2D(Collider2D collider)
    {
        PathNode node = collider.GetComponentInParent<PathNode>();
        if (node != null)
        {
            nodes.Add(node);
            latestNode = node;
        }
    }

    // Handle trigger exit to find out when we're not in any navigation nodes.
    void OnTriggerExit2D(Collider2D collider)
    {
        PathNode node = collider.GetComponentInParent<PathNode>();
        if (node != null)
        {
            nodes.Remove(node);
        }
    }
}
