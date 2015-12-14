using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class SandwichPartPickup : MonoBehaviour
{
    [SerializeField] public SandwichInventory.SandwichPart Type;
    static SandwichResources resources;

    // Use this for initialization
    void Start()
    {
        resources = FindObjectOfType<SandwichResources>();
        mSprites = new Sprite[(int)SandwichInventory.SandwichPart.NumParts];
        GetComponent<SpriteRenderer>().sprite = resources.SandwichSprites[(int)Type];
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

    Sprite[] mSprites;
}
