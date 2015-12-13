using UnityEngine;
using System.Collections;

public class JumpNode : PathNode
{
    [SerializeField] public PathNode destination;

    protected override void AddDefaultNodes()
    {
        base.AddDefaultNodes();

        // Destination nodes may have no further destination.
        if (destination != null)
        {
            nodes.Add(destination);
        }
    }
}
