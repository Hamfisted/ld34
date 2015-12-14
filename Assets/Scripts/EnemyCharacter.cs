using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyCharacter : Character
{
    [SerializeField] public float JumpPeakOffset = 2f;
    [SerializeField] private PlayerCharacter player;

    // Enemy gameplay state.
    private GameObject target;

    void Update()
    {
        UpdateAnimator();
    }

    // Per frame character update.
    protected virtual void UpdateAnimator()
    {
        // animator.SetBool("OnGround", true);
        // animator.SetTrigger("Chomp");
    }
}
