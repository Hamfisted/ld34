using UnityEngine;
using System.Collections;

public class SandwichPartPickup : MonoBehaviour
{
    [SerializeField] public SandwichInventory.SandwichPart mType;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    //==========================//
    void OnTriggerEnter2D(Collider2D other)
    {
        var inv = other.gameObject.GetComponent<SandwichInventory>();
        if (inv && inv.AcceptSandwichPart(mType))
        {
            Destroy(gameObject);
        }
    }
}
