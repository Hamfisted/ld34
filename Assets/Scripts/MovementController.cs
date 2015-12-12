using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (BoxCollider))]
public class MovementController : MonoBehaviour
{
    // Character being controlled.
    protected Character character;

    protected void Initialize()
    {
        Rigidbody body = GetComponent<Rigidbody>();
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
    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            character.OnContactEvent(contact.normal);            
        }
    }
}
