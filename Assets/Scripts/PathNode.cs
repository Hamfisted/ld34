using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof (Collider2D))]
public class PathNode : MonoBehaviour
{
    // Explicit walk nodes.
    [SerializeField] List<PathNode> explicitWalkNodes;

    // Nodes that characters can blindly walk to from this any point in this one.
    public HashSet<PathNode> nodes { get; protected set; }

    // Node debugging.
    static int nextIndex = 0;
    int index;

    void Start()
    {
        nodes = new HashSet<PathNode>();
        index = nextIndex++;
        AddDefaultNodes();
    }

    protected virtual void AddDefaultNodes()
    {
        foreach (PathNode node in explicitWalkNodes)
        {
            nodes.Add(node);
        }
        PathManager.instance.AddNode(this);
    }

    // Add overlapping nodes as walkable.
    void OnTriggerEnter2D(Collider2D collider)
    {
        PathNode node = collider.GetComponentInParent<PathNode>();
        if ((node != null) && (node != this))
        {
            nodes.Add(node);
        }
    }
}
