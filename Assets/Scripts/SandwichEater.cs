using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class SandwichEater : MonoBehaviour
{
    [SerializeField] public int MaxFat = 1;
    [SerializeField] public float MaxAngerHungryTime = 15f;
    [SerializeField] public float eatHungerTimeSubtract = 2f;

    protected Animator animator;

    public float timeSinceFood { get; private set; }

    // Use this for initialization
    void Start()
    {
        mFatLevel = 0;
        animator = GetComponent<Animator>();
        timeSinceFood = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceFood += Time.deltaTime;
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
