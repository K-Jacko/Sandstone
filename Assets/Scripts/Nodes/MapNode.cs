using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
   private Grid<MapNode> grid;
   public int x;
   public int y;

   public int gCost;
   public int hCost;
   public int fCost;

   public Vector3 raycastY;

   public MapNode cameFromNode;

   public MapNode(Grid<MapNode> grid, int x, int y)
   {
      this.grid = grid;
      this.x = x;
      this.y = y;
   }

   public void CalculateFCost()
   {
      fCost = gCost + hCost;
   }
}
