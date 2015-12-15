using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SandwichEater))]
public class EnemyCharacter : Character
{
    [SerializeField] public float JumpPeakOffset = 2f;
    [SerializeField] private PlayerCharacter player;
    [SerializeField] private float AngerSmoothTime = 2f;

    // Enemy gameplay state.
    private GameObject target;
    public bool isDead = false;
    private Vector2 spawnPosition;
    private SandwichEater eater;
    private float angerVelocity;

    void Start()
    {
        StartCharacter();
        spawnPosition = body.position;
        eater = GetComponent<SandwichEater>();
        angerVelocity = 0f;
    }

    void Update()
    {
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        float angerFactor = Mathf.Sqrt(eater.timeSinceFood / eater.MaxAngerHungryTime);
        float targetX = Mathf.Lerp(spawnPosition.x, player.GetBodyPosition().x, angerFactor);
        float currentX = Mathf.SmoothDamp(body.position.x, targetX, ref angerVelocity, AngerSmoothTime);
        Vector2 position = new Vector2(currentX, body.position.y);

        body.MovePosition(position);
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
