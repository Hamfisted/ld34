using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    // Component managers.
    protected Animator animator;
    protected Rigidbody2D body;

    protected void StartCharacter()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }
}
