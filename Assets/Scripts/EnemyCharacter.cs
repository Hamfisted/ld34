using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyCharacter : Character
{
    [SerializeField] public float JumpPeakOffset = 2f;
    [SerializeField] private PlayerCharacter player;

    // Enemy gameplay state.
    private GameObject target;
    public bool isDead = false;

    void Start()
    {
        StartCharacter();
    }

    void Update()
    {
        UpdateAnimator();
    }

    // Per frame character update.
    protected virtual void UpdateAnimator()
    {
        animator.SetBool("IsDead", isDead);
    }

    public void Die()
    {
        isDead = true;
    }
}
