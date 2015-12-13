using UnityEngine;
using System.Collections.Generic;

public class PathManager : MonoBehaviour
{
    // Singleton instance for a given level.
    public static PathManager instance;

    // Path nodes members.
    private List<PathNode> nodes;

    void Awake()
    {
        instance = this;
        nodes = new List<PathNode>();
    }

    // Reset the node list before they start.
    void OnEnable()
    {
        nodes.Clear();
    }

    // Add a node to the manager.
    public void AddNode(PathNode node)
    {
        nodes.Add(node);
    }

    // Helper structure for searching.
    struct PathLink
    {
        public int lastIndex;
        public PathNode node;

        public PathLink(int newLastIndex, PathNode newNode)
        {
            lastIndex = newLastIndex;
            node = newNode;
        }
    }

    // Search for a path from one node to another.
    // Currently just breadth first search.
    // Returns empty list if no path can be found.
    List<PathNode> Search(PathNode start, PathNode end)
    {
        List<PathNode> result = new List<PathNode>();
        List<PathLink> open = new List<PathLink>();
        HashSet<PathNode> explored = new HashSet<PathNode>();

        // Obvious case:
        if (start == end)
        {
            result.Add(start);
            return result;
        }

        // Keep going until we've search them all.
        PathLink firstLink = new PathLink(-1, start);
        open.Add(firstLink);
        explored.Add(start);
        for (int i = 0; i < open.Count; ++i)
        {
            PathLink current = open[i];
            PathNode currentNode = current.node;

            // Found the end? Build the path.
            if (currentNode == end)
            {
                result.Add(end);
                int rofl = 0;
                for (int j = current.lastIndex; j != -1; j = current.lastIndex)
                {
                    if (rofl++ > 30) break;
                    current = open[j];
                    result.Insert(0, current.node);
                }
                return result;
            }

            // Add its children to the open list.
            int lol = 0;
            foreach (PathNode child in currentNode.nodes)
            {
                if (!explored.Contains(child))
                {
                    explored.Add(child);
                    PathLink newLink = new PathLink(i, child);
                    open.Add(newLink);
                }
                if (lol++ > 30) break;
            }
            if (i > 30) break;
        }

        return result;
    }

    // Test path finding.
    bool find = false;
    void Update()
    {
        if (find) return;
        find = true;
        // Search from every node to every other node.
        foreach (PathNode node in nodes)
        {
            foreach (PathNode other in nodes)
            {
                // Get the path.
                string resultString = "Path from " + node + " to " + other + ": ";
                List<PathNode> path = Search(node, other);
                if (path.Count == 0)
                {
                    resultString += "(none)";
                }
                else
                {
                    resultString += path[0];
                    for (int i = 1; i < path.Count; ++i)
                    {
                        resultString += " -> " + path[i];
                    }
                }
                Debug.Log(resultString);
            }
        }
    }
}
