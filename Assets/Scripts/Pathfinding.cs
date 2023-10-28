using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Pathfinding
{
    public static List<NodeBase> FindPath (NodeBase startNode, NodeBase targetNode)
    {
        var toSearch = new List<NodeBase>() { startNode };
        var processed = new List<NodeBase>();

        while (toSearch.Any())
        {
            var current = toSearch[0];
            foreach(var t in toSearch)
            {
                if (t.F < current.F || t.F == current.F && t.H < current.H) current = t;
            }

            processed.Add(current);
            toSearch.Remove(current);

            if (current == targetNode)
            {
                var currentpathTile = targetNode;
                var path = new List<NodeBase>();
                var count = 100;

                while(currentpathTile != startNode)
                {
                    path.Add(currentpathTile);
                    currentpathTile = currentpathTile.Connection;
                    count--;
                    if (count < 0)
                    {
                        Debug.Log("Cant find path");
                        return new List<NodeBase>();
                    }
                }

                return path;
            }

            foreach (var neighbor in current.Neighbors.Where(t => t.Walkable && !processed.Contains(t)))
            {
                bool inSearch = toSearch.Contains(neighbor);

                float costToNeighbor = current.G + current.coord.GetDistance(neighbor.coord);

                if (!inSearch || costToNeighbor < neighbor.G)
                {
                    neighbor.SetG(costToNeighbor);
                    neighbor.SetConnection(current);

                    if (!inSearch)
                    {
                        neighbor.SetH(neighbor.coord.GetDistance(targetNode.coord));
                        toSearch.Add(neighbor);
                    }
                }
            }
        }
        return new List<NodeBase>();
    }
}
