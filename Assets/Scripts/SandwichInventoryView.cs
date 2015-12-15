using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SandwichInventoryView : MonoBehaviour {

    [SerializeField] Image FullSandwich;
    [SerializeField] Text FullSandwichCount;
    [SerializeField] Image Bread1;
    [SerializeField] Image Cheese;
    [SerializeField] Image Meat;
    [SerializeField] Image Lettuce;
    [SerializeField] Image Tomato;
    [SerializeField] Image Bread2;

    static Color GreyColor = new Color(0.5f, 0.5f, 0.5f);
    static Color WhiteColor = new Color(1.0f, 1.0f, 1.0f);

    // Use this for initialization
    void Start () {
        FullSandwich.color = GreyColor;
        FullSandwichCount.text = "0";
        Bread1.color = GreyColor;
        Cheese.color = GreyColor;
        Meat.color = GreyColor;
        Lettuce.color = GreyColor;
        Tomato.color = GreyColor;
        Bread2.color = GreyColor;

    }

    // Update is called once per frame
    void Update () {
    }

    public void UpdateSandwichState(int numSandwiches, int[] sandwichInventory)
    {
        FullSandwich.color = (numSandwiches > 0) ? WhiteColor : GreyColor;
        FullSandwichCount.text = numSandwiches.ToString();

        int breadCount = sandwichInventory[(int)SandwichInventory.SandwichPart.Bread];
        int cheeseCount = sandwichInventory[(int)SandwichInventory.SandwichPart.Cheese];
        int meatCount = sandwichInventory[(int)SandwichInventory.SandwichPart.Meat];
        int lettuceCount = sandwichInventory[(int)SandwichInventory.SandwichPart.Lettuce];
        int tomatoCount = sandwichInventory[(int)SandwichInventory.SandwichPart.Tomato];

        Bread1.color = (breadCount > 0) ? WhiteColor : GreyColor;
        Bread2.color = (breadCount > 1) ? WhiteColor : GreyColor;
        Cheese.color = (cheeseCount > 0) ? WhiteColor : GreyColor;
        Meat.color = (meatCount > 0) ? WhiteColor : GreyColor;
        Lettuce.color = (lettuceCount > 0) ? WhiteColor : GreyColor;
        Tomato.color = (tomatoCount > 0) ? WhiteColor : GreyColor;
    }
}
