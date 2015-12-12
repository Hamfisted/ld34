using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    // Character being controlled.
    protected Character character;

    protected void Initialize()
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        character = new Character(body);
    }

    // Notify character of ground and wall collision.
    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            character.OnContactEvent(contact.normal);            
        }
    }

    // Notify persisting collisions to character.
    void OnCollisionStay2D(Collision2D collision)
    {
        OnCollisionEnter2D(collision);
    }
}
