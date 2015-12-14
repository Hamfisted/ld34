using UnityEngine;
using System.Collections;

public class SandwichInventory : MonoBehaviour
{
    public enum SandwichPart
    {
        Bread = 0,
        Cheese,
        Meat,
        Lettuce,
        Tomato,
        NumParts
    };
    [SerializeField] public int NumRequiredBread = 2;
    [SerializeField] public int NumRequiredCheese = 1;
    [SerializeField] public int NumRequiredMeat = 1;
    [SerializeField] public int NumRequiredLettuce = 1;
    [SerializeField] public int NumRequiredTomato = 1;
    [SerializeField] public Object SandwichType;

    // Use this for initialization
    void Start()
    {
        mSandwichParts = new int[(int)SandwichPart.NumParts];
        mSandwichPartLimits = new int[(int)SandwichPart.NumParts];
        mSandwichPartLimits[(int)SandwichPart.Bread] = NumRequiredBread;
        mSandwichPartLimits[(int)SandwichPart.Cheese] = NumRequiredCheese;
        mSandwichPartLimits[(int)SandwichPart.Meat] = NumRequiredMeat;
        mSandwichPartLimits[(int)SandwichPart.Lettuce] = NumRequiredLettuce;
        mSandwichPartLimits[(int)SandwichPart.Tomato] = NumRequiredTomato;
        mNumSandwiches = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool AcceptSandwichPart(SandwichPart type)
    {
        if (type >= SandwichPart.NumParts)
        {
            return (false);
        }
        if (mSandwichParts[(int)type] >= mSandwichPartLimits[(int)type])
        {
            return (false);
        }
        mSandwichParts[(int)type] += 1;
        if (HasEnoughPartsForSandwich())
        {
            MakeSandwich();
        }
        return (true);
    }

    public bool UseSandwich()
    {
        if (mNumSandwiches > 0)
        {
            Instantiate(SandwichType, transform.position, transform.rotation);
            mNumSandwiches -= 1;
            return (true);
        }
        return (false);
    }

    bool HasEnoughPartsForSandwich()
    {
        if ((mSandwichParts[(int)SandwichPart.Bread] >= mSandwichPartLimits[(int)SandwichPart.Bread]) &&
            (mSandwichParts[(int)SandwichPart.Cheese] >= mSandwichPartLimits[(int)SandwichPart.Cheese]) &&
            (mSandwichParts[(int)SandwichPart.Meat] >= mSandwichPartLimits[(int)SandwichPart.Meat]) &&
            (mSandwichParts[(int)SandwichPart.Lettuce] >= mSandwichPartLimits[(int)SandwichPart.Lettuce]) &&
            (mSandwichParts[(int)SandwichPart.Tomato] >= mSandwichPartLimits[(int)SandwichPart.Tomato]))
        {
            return (true);
        }
        return (false);
    }

    void MakeSandwich()
    {
        mSandwichParts = new int[(int)SandwichPart.NumParts];
        mNumSandwiches += 1;
    }

    int[] mSandwichParts;
    int[] mSandwichPartLimits;
    int mNumSandwiches;
}
