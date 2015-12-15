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
        // LOL
        EnemyCharacter character = FindObjectOfType<EnemyCharacter>();
        SandwichEater eater = FindObjectOfType<SandwichEater>();
        if (character && eater)
        {
            Vector2 pos = transform.position;
            Vector2 diff = (character.GetBodyPosition() - pos);
            if (Mathf.Abs(diff.x) <= 1f * character.gameObject.transform.localScale.x && Mathf.Abs(diff.y) <= 2f * character.gameObject.transform.localScale.y)
            {
                eater.EatSandwich();
                Destroy(gameObject);
            }
        }            
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
