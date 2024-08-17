using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTile: MonoBehaviour
{
    public bool isWalkable=true;
    public int gridX, gridY; 
    public int cellX, cellY;
    public List<WorldTile> Neighbours;
    public WorldTile parent;
    public int gCost; //The cost from the start node to the current node
    public int hCost; //The heuristic cost from the current node to the end node
    public int fCost //Sum of G and H
    {
        get 
        {
            return gCost + hCost; 
        } 
    } 
    public WorldTile(bool _isWalkable, int _gridX, int _gridY)
    {
        isWalkable = _isWalkable;
        this.gridX = _gridX;
        this.gridY = _gridY;

    }
    
}
