using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class SandwichEater : MonoBehaviour
{
    [SerializeField] public int MaxFat = 3;
    [SerializeField] public float MaxAngerHungryTime = 60f;
    [SerializeField] public float eatHungerTimeSubtract = 10f;
    [SerializeField] private float MaxScale = 1.5f;

    protected Animator animator;

    public float timeSinceFood { get; private set; }
    private float scaleVelocity;

    // Use this for initialization
    void Start()
    {
        mFatLevel = 0;
        animator = GetComponent<Animator>();
        timeSinceFood = 0f;
        scaleVelocity = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceFood = Mathf.Min(MaxAngerHungryTime, timeSinceFood + Time.deltaTime);
        float scale = Mathf.Lerp(1f, MaxScale, (float)mFatLevel / (float)MaxFat);
        transform.localScale = new Vector3(1f, 1f, 1f) * Mathf.SmoothDamp(transform.localScale.y, scale, ref scaleVelocity, 1f);
    }

    public void EatPlayer()
    {
        animator.SetTrigger("Chomp");
        animator.SetLayerWeight(1, 1f);
    }

    public void EatSandwich()
    {
        mFatLevel += 1;
        animator.SetTrigger("Chomp");
        animator.SetLayerWeight(1, 1);
        timeSinceFood = Mathf.Max(0f, timeSinceFood - eatHungerTimeSubtract);
        if(mFatLevel >= MaxFat)
        {
            // Destroy(gameObject);
            GetComponent<EnemyCharacter>().Die();
        }
    }

    int mFatLevel;
}
