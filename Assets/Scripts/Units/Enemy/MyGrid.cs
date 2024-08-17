using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MyGrid : MonoBehaviour
{
    public Grid gridBase;
    public Tilemap floor;                         // walkable tilemap
    public List<Tilemap> obstacleLayers;   // all layers that contain objects to navigate around
    public GameObject gridNode;            // where the generated tiles will be stored
    public GameObject nodePrefab;          // world tile prefab
    public List<WorldTile> path;

    //these are the bounds of where we are searching in the world for tiles, have to use world coords to check for tiles in the tile map
    public int scanStartX = -50, scanStartY = -50, scanFinishX = 50, scanFinishY = 50, gridSizeX, gridSizeY;

    private List<GameObject> unsortedNodes;   // all the nodes in the world
    public GameObject[,] nodes;           // sorted 2d array of nodes, may contain null entries if the map is of an odd shape e.g. gaps
    private int gridBoundX = 0, gridBoundY = 0;
void Start()
    {
        gridSizeX = Mathf.Abs(scanStartX) + Mathf.Abs(scanFinishX);
        gridSizeY = Mathf.Abs(scanStartY) + Mathf.Abs(scanFinishY);
    }
    private void Awake()
    {
        unsortedNodes = new List<GameObject>();
        CreateGrid();
    }
    private void Update()
    {
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSizeX, gridSizeY));

        if (path != null)
        {
            foreach (WorldTile n in path)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(new Vector3(n.cellX+0.5f,n.cellY+0.5f), new Vector3(1f,1f));
            }
        }
    }
    public List<WorldTile> getNeighbours(int x, int y, int width, int height)
    {
        List<WorldTile> Neighbours = new List<WorldTile>();

        if (x > 0 && x < width - 1)
        {
            if (y > 0 && y < height - 1)
            {
                if (nodes[x + 1, y] != null)
                {
                    if (nodes[x + 1, y].TryGetComponent<WorldTile>(out var wt1)) Neighbours.Add(wt1);
                }

                if (nodes[x - 1, y] != null)
                {
                    if (nodes[x - 1, y].TryGetComponent<WorldTile>(out var wt2)) Neighbours.Add(wt2);
                }

                if (nodes[x, y + 1] != null)
                {
                    if (nodes[x, y + 1].TryGetComponent<WorldTile>(out var wt3)) Neighbours.Add(wt3);
                }

                if (nodes[x, y - 1] != null)
                {
                    if (nodes[x, y - 1].TryGetComponent<WorldTile>(out var wt4)) Neighbours.Add(wt4);
                }
            }
            else if (y == 0)
            {
                if (nodes[x + 1, y] != null)
                {
                    if (nodes[x + 1, y].TryGetComponent<WorldTile>(out var wt1)) Neighbours.Add(wt1);
                }

                if (nodes[x - 1, y] != null)
                {
                    if (nodes[x - 1, y].TryGetComponent<WorldTile>(out var wt2)) Neighbours.Add(wt2);
                }

                if (nodes[x, y + 1] == null)
                {
                    if (nodes[x, y + 1].TryGetComponent<WorldTile>(out var wt3)) Neighbours.Add(wt3);
                }
            }
            else if (y == height - 1)
            {
                if (nodes[x, y - 1] != null)
                {
                    if (nodes[x, y - 1].TryGetComponent<WorldTile>(out var wt4)) Neighbours.Add(wt4);
                }
                if (nodes[x + 1, y] != null)
                {
                    if (nodes[x + 1, y].TryGetComponent<WorldTile>(out var wt1)) Neighbours.Add(wt1);
                }

                if (nodes[x - 1, y] != null)
                {
                    if (nodes[x - 1, y].TryGetComponent<WorldTile>(out var wt2)) Neighbours.Add(wt2);
                }
            }
        }
        else if (x == 0)
        {
            if (y > 0 && y < height - 1)
            {
                if (nodes[x + 1, y] != null)
                {
                    if (nodes[x + 1, y].TryGetComponent<WorldTile>(out var wt1)) Neighbours.Add(wt1);
                }

                if (nodes[x, y - 1] != null)
                {
                    if (nodes[x, y - 1].TryGetComponent<WorldTile>(out var wt4)) Neighbours.Add(wt4);
                }

                if (nodes[x, y + 1] != null)
                {
                    if (nodes[x, y + 1].TryGetComponent<WorldTile>(out var wt3)) Neighbours.Add(wt3);
                }
            }
            else if (y == 0)
            {
                if (nodes[x + 1, y] != null)
                {
                    if (nodes[x + 1, y].TryGetComponent<WorldTile>(out var wt1)) Neighbours.Add(wt1);
                }

                if (nodes[x, y + 1] != null)
                {
                    if (nodes[x, y + 1].TryGetComponent<WorldTile>(out var wt3)) Neighbours.Add(wt3);
                }
            }
            else if (y == height - 1)
            {
                if (nodes[x + 1, y] != null)
                {
                    if (nodes[x + 1, y].TryGetComponent<WorldTile>(out var wt1)) Neighbours.Add(wt1);
                }

                if (nodes[x, y - 1] != null)
                {
                    if (nodes[x, y - 1].TryGetComponent<WorldTile>(out var wt4)) Neighbours.Add(wt4);
                }
            }
        }
        else if (x == width - 1)
        {
            if (y > 0 && y < height - 1)
            {
                if (nodes[x - 1, y] != null)
                {
                    if (nodes[x - 1, y].TryGetComponent<WorldTile>(out var wt2)) Neighbours.Add(wt2);
                }

                if (nodes[x, y + 1] != null)
                {
                    if (nodes[x, y + 1].TryGetComponent<WorldTile>(out var wt3)) Neighbours.Add(wt3);
                }

                if (nodes[x, y - 1] != null)
                {
                    if (nodes[x, y - 1].TryGetComponent<WorldTile>(out var wt4)) Neighbours.Add(wt4);
                }
            }
            else if (y == 0)
            {
                if (nodes[x - 1, y] != null)
                {
                    if (nodes[x - 1, y].TryGetComponent<WorldTile>(out var wt2)) Neighbours.Add(wt2);
                }
                if (nodes[x, y + 1] != null)
                {
                    if (nodes[x, y + 1].TryGetComponent<WorldTile>(out var wt3)) Neighbours.Add(wt3);
                }
            }
            else if (y == height - 1)
            {
                if (nodes[x - 1, y] != null)
                {
                    if (nodes[x - 1, y].TryGetComponent<WorldTile>(out var wt2)) Neighbours.Add(wt2);
                }

                if (nodes[x, y - 1] != null)
                {
                    if (nodes[x, y - 1].TryGetComponent<WorldTile>(out var wt4)) Neighbours.Add(wt4);
                }
            }
        }
        return Neighbours;
    }
    public void CreateGrid()
    {
        int gridX = 0, gridY = 0;
        bool foundTileOnLastPass = false;
        for (int x = scanStartX; x < scanFinishX; x++)
        {
            for (int y = scanStartY; y < scanFinishY; y++)
            {
                TileBase tb = floor.GetTile(new Vector3Int(x, y, 0));
                if (tb != null)
                {
                    bool foundObstacle = false;
                    foreach (Tilemap t in obstacleLayers)
                    {
                        TileBase tb2 = t.GetTile(new Vector3Int(x, y, 0));
                        if (tb2 != null) foundObstacle = true;
                    }

                    Vector3 worldPosition = new Vector3(x + 0.5f + gridBase.transform.position.x, y + 0.5f + gridBase.transform.position.y, 0);
                    GameObject node = (GameObject)Instantiate(nodePrefab, worldPosition, Quaternion.Euler(0, 0, 0));
                    Vector3Int cellPosition = floor.WorldToCell(worldPosition);
                    WorldTile wt = node.GetComponent<WorldTile>();
                    wt.gridX = gridX; wt.gridY = gridY; wt.cellX = cellPosition.x; wt.cellY = cellPosition.y;
                    node.transform.parent = gridNode.transform;

                    if (!foundObstacle)
                    {
                        foundTileOnLastPass = true;
                        node.name = "Walkable_" + gridX.ToString() + "_" + gridY.ToString();
                    }
                    else
                    {
                        foundTileOnLastPass = true;
                        node.name = "Unwalkable_" + gridX.ToString() + "_" + gridY.ToString();
                        wt.isWalkable = false;
                        node.GetComponent<SpriteRenderer>().color = Color.red;
                    }

                    unsortedNodes.Add(node);

                    gridY++;
                    if (gridX > gridBoundX)
                        gridBoundX = gridX;

                    if (gridY > gridBoundY)
                        gridBoundY = gridY;
                }
            }
            if (foundTileOnLastPass)
            {
                gridX++;
                gridY = 0;
                foundTileOnLastPass = false;
            }       
        }
        nodes = new GameObject[gridBoundX + 1, gridBoundY + 1];

        foreach (GameObject g in unsortedNodes)
        {
            WorldTile wt = g.GetComponent<WorldTile>();
            nodes[wt.gridX, wt.gridY] = g;
        }

        for (int x = 0; x < gridBoundX; x++)
        {
            for (int y = 0; y < gridBoundY; y++)
            {
                if (nodes[x, y] != null)
                {
                    WorldTile wt = nodes[x, y].GetComponent<WorldTile>();
                    wt.Neighbours = getNeighbours(x, y, gridBoundX, gridBoundY);
                }
            }
        }
    }

    public WorldTile GetWorldTileByCellPosition(Vector3 worldPosition)
    {
        Vector3Int cellPosition = floor.WorldToCell(worldPosition);
        WorldTile wt = null;
        for (int x = 0; x < gridBoundX; x++)
        {
            for (int y = 0; y < gridBoundY; y++)
            {
                if (nodes[x, y] != null)
                {
                    WorldTile _wt = nodes[x, y].GetComponent<WorldTile>();

                    // we are interested in walkable cells only
                    if (_wt.isWalkable && _wt.cellX == cellPosition.x && _wt.cellY == cellPosition.y)
                    {
                        wt = _wt;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        return wt;
    }


}
