using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNode
{
    public Vector3 raycastLocation;
    public List<GameObject> props;
    
    private Grid<SpawnNode> grid;
    public int x;
    public int y;

    public SpawnNode(Vector3 raycastLocation)
    {
        this.raycastLocation = raycastLocation;
    }

    public SpawnNode(Vector3 raycastLocation, int numberOfPossibleProps)
    {
        this.raycastLocation = raycastLocation;
        this.props = new List<GameObject>(numberOfPossibleProps);
    }

    public SpawnNode(Grid<SpawnNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return $"RaycastLocation is {raycastLocation}";
    }
}
