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

    // Per-frame update.
    protected void UpdateController()
    {
        character.Update();
    }

    // Physics update.
    protected void MoveController()
    {
        character.Move();
    }

    // Update grounded state.
    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            character.OnContactEvent(contact.normal);            
        }
    }
}
