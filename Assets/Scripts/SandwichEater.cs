using UnityEngine;
using System.Collections;

public class SandwichEater : MonoBehaviour
{
    [SerializeField] public int MaxFat = 1;
    // Use this for initialization
    void Start()
    {
        mFatLevel = 0;
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
        }
    }

    int mFatLevel;
}
