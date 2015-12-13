using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyCharacter : Character
{
    [SerializeField] public float JumpPeakOffset = 2f;
    [SerializeField] private Character player;

    // Enemy gameplay state.
    private GameObject target;

    void Start()
    {
        // Start targetting player.
        target = player.gameObject;

        StartCharacter();
        StartCoroutine(WaitForNode());
    }

    // Per-frame controller update.
    void Update()
    {
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        MoveCharacter();
    }

    // Wait until we're in a navigation node so we know where to path from.
    IEnumerator WaitForNode()
    {
        while (latestNode == null)
        {
            yield return null;
        }
        StartCoroutine(ChaseTarget());
    }

    // Chase the target.
    IEnumerator ChaseTarget()
    {
        // Find a path to target.
        while (true)
        {
            if (target == player.gameObject)
            {
                // Path to last node we saw the player in.
                List<PathNode> path = PathManager.instance.Search(latestNode, player.latestNode);

                // Find the first node we're not already in.
                int nextIndex;
                for (nextIndex = 1; nextIndex < path.Count; ++nextIndex)
                {
                    PathNode node = path[nextIndex];
                    if (!nodes.Contains(node))
                    {
                        break;
                    }
                }

                Vector3 moveDirection;
                if (nextIndex >= path.Count)
                {
                    // We're in the same node as the player or no path found (unlikely unless we messed up levels), so charge them!
                    moveDirection = (player.transform.position - transform.position);
                }
                else
                {
                    PathNode current = path[nextIndex - 1];
                    PathNode next = path[nextIndex];

                    // If the current node is a jump, next is destination, and we're moving towards it, jump.
                    JumpNode jump = current as JumpNode;
                    moveDirection = (next.transform.position - transform.position);

                    if ((jump != null) && (next == jump.destination) && (moveDirection.x * physics.GetVelocity().x >= 0f))
                    {
                        do
                        {
                            yield return null;
                        } while (!physics.isGrounded);

                        JumpToTarget(next);
                        StartCoroutine(Jumping());
                        break;
                    }
                }

                // Move horizontal towards target.
                moveDirection.Scale(new Vector3(1f, 0f, 0f));
                moveDirection.Normalize();
                moveDirection.x *= Acceleration;
                if (!physics.isGrounded)
                {
                    moveDirection.x *= AirControl;
                }
                physics.HorizontalAccelerate(moveDirection.x);
            }

            yield return null;
        }
    }

    IEnumerator Jumping()
    {
        // Just wait to land.
        while (!physics.isGrounded || (physics.GetVelocity().y > Mathf.Epsilon))
        {
            yield return null;
        }
        StartCoroutine(ChaseTarget());
    }

    // Helper for calculating jump velocity.
    void JumpToTarget(PathNode node)
    {
        Vector3 difference = (node.transform.position - transform.position);
        float peakHeight = Mathf.Max(difference.y + JumpPeakOffset, 0f);

        // What velocity do we need to reach that height?
        float gravity = Physics2D.gravity.y;
        Vector2 velocity = new Vector2(0f, Mathf.Sqrt(-2f * Physics2D.gravity.y * peakHeight));

        // Calculate how much time it'll take to reach target height, then get horizontal velocity from that.
        float determinant = (velocity.y * velocity.y) - (2f * Physics2D.gravity.y * JumpPeakOffset);
        if (determinant >= 0f)
        {
            float first = (-velocity.y + Mathf.Sqrt(determinant)) / Physics2D.gravity.y;
            float second = (-velocity.y - Mathf.Sqrt(determinant)) / Physics2D.gravity.y;
            float time = Mathf.Max(first, second);
            if (time > Mathf.Epsilon)
            {
                velocity.x = difference.x / time;
            }
        }
        physics.SetVelocity(velocity);
    }
}
