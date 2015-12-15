using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Platform : MonoBehaviour
{
    public Rigidbody2D body;
    public BoxCollider2D box { get; private set; }

    void Start()
    {
        body = GetComponentInParent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
    }

    // Move the platform at a given rate.
    public void Move(Vector2 displacement)
    {
        Vector2 newPosition = body.position + displacement;
        body.MovePosition(newPosition);
    }
}
