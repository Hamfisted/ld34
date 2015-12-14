using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Platform : MonoBehaviour
{
    public Rigidbody2D body;
    public BoxCollider2D box { get; private set; }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
    }

    // Move the platform at a given rate.
    public void Move(Vector2 displacement)
    {
        Vector2 newPosition = body.position + displacement;
        body.MovePosition(newPosition);
    }
}
