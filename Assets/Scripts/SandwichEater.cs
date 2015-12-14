using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class SandwichEater : MonoBehaviour
{
    [SerializeField] public int MaxFat = 1;

    protected Animator animator;

    // Use this for initialization
    void Start()
    {
        mFatLevel = 0;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void EatSandwich()
    {
        mFatLevel += 1;
        if(mFatLevel >= MaxFat)
        {
            Destroy(gameObject);
        } else {
            animator.SetTrigger("Chomp");
            animator.SetLayerWeight(1, 1);
        }
    }

    int mFatLevel;
}
