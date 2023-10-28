using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Grid movementGrid;
    public Grid visualGrid;
    public Tilemap groundTiles;
    public Tilemap machineTiles;
    [SerializeField] int widthFromCenter, lengthFromCenter;

    public Dictionary<Vector2Int, NodeBase> nodes = new Dictionary<Vector2Int, NodeBase>();

    // Node with machine
    public Dictionary<Vector2Int, NodeBase> machineNodes = new Dictionary<Vector2Int, NodeBase>();

    public NodeBase GetGridPosAt(Vector2Int pos) => nodes.TryGetValue(pos, out NodeBase value) ? value : null;

    public NodeBase GetGridPosAt(Vector3Int pos) => GetGridPosAt(((Vector2Int)pos));

    

    private void InitializeGrid()
    {
        for (int i = -widthFromCenter; i <= widthFromCenter; i++)
        {
            for (int j = -lengthFromCenter; j <= lengthFromCenter; j++)
            {
                bool isGround = groundTiles.HasTile(new Vector3Int(i, j));

                NodeBase newNode = new NodeBase(i, j, isGround);

                Vector2Int newVector = new Vector2Int(i, j);

                nodes.Add(newVector, newNode);
            }
        }
    }

    private void InitializeGridData()
    {
        for (int i = -widthFromCenter; i <= widthFromCenter; i++)
        {
            for (int j = -lengthFromCenter; j <= lengthFromCenter; j++)
            {
                // Cache negihbors for each tiles
                NodeBase newNode = nodes.TryGetValue(new Vector2Int(i, j), out NodeBase value) ? value : null;

                if (newNode != null)
                {
                    newNode.CacheNeighbors();
                }

                // Check node if it has machine (in Z = 0, might change in the future)

                //if (machineTiles.HasTile(new Vector3Int(i, j)))
                //{
                //    Debug.Log("Has Machine");
                //    newNode.SetHasMachine();
                //    machineNodes.Add(new Vector2Int(i, j), newNode);
                //}
            }
        }
    }


    private void Start()
    {     
        // Initialize the grids
        InitializeGrid();

        // Check all initialized grids
        InitializeGridData();

    }
}
