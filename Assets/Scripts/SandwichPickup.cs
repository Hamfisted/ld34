using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class SandwichPickup : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var eater = other.gameObject.GetComponent<SandwichEater>();
        if (eater)
        {
            eater.EatSandwich();
            Destroy(gameObject);
        }
    }
}
