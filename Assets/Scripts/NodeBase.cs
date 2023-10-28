using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeBase
{
    
    
    public bool Walkable { get; private set; }
    //public bool HasMachine { get; private set; }
    public NodeBase Connection { get; private set; }

    public float G { get; private set; }
    public float H { get; private set; }
    public float F => G + H;

    public NodeBase(int x, int y, bool isWalkable)
    {
        coord.pos.x = x;
        coord.pos.y = y;

        Walkable = isWalkable;
    }

    public void SetWalkable(bool x) => Walkable = x;

    //public void SetHasMachine(bool x) => HasMachine = x;

    public void SetConnection(NodeBase node) => Connection = node;

    //public void SetHasMachine()
    //{
    //    SetWalkable(false);
    //    HasMachine = false;

    //}

    public void SetG(float g) => G = g;
    public void SetH(float h) => H = h;

    public List<NodeBase> Neighbors { get; private set; }

    private static readonly List<Vector2Int> dirs = new List<Vector2Int>() { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };

    public void CacheNeighbors()
    {
        Neighbors = new List<NodeBase>();

        var gridManager = ServiceLocator.Instance.gridManager;

        foreach (var node in dirs.Select(dir => gridManager.GetGridPosAt(coord.pos + dir)).Where(node => node != null && node.Walkable == true))
        {
            Neighbors.Add(node);
        }
    }

    public Coord coord;
    public struct Coord
    {
        public Vector2Int pos;

        public int GetDistance(Vector3Int other)
        {
            Coord newCoord = new Coord
            {
                pos = new Vector2Int(other.x, other.y)
            };

            return GetDistance(newCoord);
        }
        public int GetDistance(Coord other)
        {
            int distance = Mathf.Abs(pos.x - other.pos.x) + Mathf.Abs(pos.y - other.pos.y);

            return distance;
        }
    }
}
