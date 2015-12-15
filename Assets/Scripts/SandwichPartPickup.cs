using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class SandwichPartPickup : MonoBehaviour
{
    public SandwichInventory.SandwichPart Type;

    // Use this for initialization
    void Start()
    {
        Type = (SandwichInventory.SandwichPart)Random.Range(0, (int)SandwichInventory.SandwichPart.NumParts);
        GetComponent<SpriteRenderer>().sprite = FindObjectOfType<SandwichResources>().SandwichSprites[(int)Type];
    }

    // Update is called once per frame
    void Update()
    {
    }

    //==========================//
    void OnTriggerEnter2D(Collider2D other)
    {
        var inv = other.gameObject.GetComponent<SandwichInventory>();
        if (inv && inv.AcceptSandwichPart(Type))
        {
            Destroy(gameObject);
        }
    }
}
