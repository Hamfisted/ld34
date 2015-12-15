using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SandwichEater))]
public class EnemyCharacter : Character
{
    [SerializeField] public float JumpPeakOffset = 2f;
    [SerializeField] private PlayerCharacter player;
    [SerializeField] private float AngerSmoothTime = 0.5f;

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
            float dampFactor = Mathf.Pow(eater.timeSinceFood / eater.MaxAngerHungryTime, 2f);
            angerFactor = Mathf.SmoothDamp(angerFactor, dampFactor, ref angerVelocity, AngerSmoothTime);
            if (angerFactor >= 0.95f && player.offPlatform && timeSinceEatPlayer >= eater.eatPlayerTimeSubtract)
            {
                timeSinceEatPlayer = 0f;
                eater.EatPlayer();
                player.TakeDamage();
            }
        }
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            if (!player.isDead)
            {
                float targetX = Mathf.Lerp(spawnPosition.x, player.GetBodyPosition().x - 0.5f, angerFactor);
                Vector2 position = new Vector2(targetX, body.position.y);

                body.MovePosition(position);
            }
            else if (body.position.x < 64f)
            {
                body.MovePosition(body.position + (Time.fixedDeltaTime * new Vector2(5f, 0f)));
            }
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
