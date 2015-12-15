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
    private float timeSinceEatPlayer;
    public float angerFactor;

    void Start()
    {
        StartCharacter();
        spawnPosition = body.position;
        eater = GetComponent<SandwichEater>();
        angerVelocity = 0f;
        timeSinceEatPlayer = 0f;
        angerFactor = 0f;
    }

    void Update()
    {
        if (!isDead)
        {
            timeSinceEatPlayer = Mathf.Min(eater.MaxAngerHungryTime, timeSinceEatPlayer + Time.deltaTime);
            angerFactor = eater.timeSinceFood / eater.MaxAngerHungryTime;

            if (angerFactor >= 1f && player.offPlatform && timeSinceEatPlayer >= eater.eatHungerTimeSubtract)
            {
                timeSinceEatPlayer = 0f;
                eater.EatPlayer();
            }
        }
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            float smoothedFactor = Mathf.Pow(angerFactor, 3f);
            float targetX = Mathf.Lerp(spawnPosition.x, player.GetBodyPosition().x - 1f, angerFactor);
            float currentX = Mathf.SmoothDamp(body.position.x, targetX, ref angerVelocity, AngerSmoothTime);
            Vector2 position = new Vector2(currentX, body.position.y);

            body.MovePosition(position);
        }
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
