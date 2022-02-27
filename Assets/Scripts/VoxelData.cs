using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelData : MonoBehaviour
{
    int[,] data = new int[,]
    {
      {
          1, 1, 1, 1, 1
      },
      {
          0, 0, 0, 1, 0
      },  
      {
          1, 1, 1, 1, 1
      },
      {
          0, 1, 0, 1, 1
      }
    };

    public int Width 
    {
        get 
        {
            return data.GetLength(0);
        }
    }

    public int Depth
    {
        get
        {
            return data.GetLength(1);
        }
    }

    public int GetCell (int x, int z)
    {
        return data [x,z];
    }

    public int GetNeighbor (int x, int z, Direction dir)
    {
        DataCorordinate offsetToCheck = offsets [(int)dir];
        DataCorordinate neighboorCoord = new DataCorordinate(x + offsetToCheck.x, 0 + offsetToCheck.y, z + offsetToCheck.z);

        if(neighboorCoord.x < 0 || neighboorCoord.x >= Width || neighboorCoord.y !=0 || neighboorCoord.z < 0 || neighboorCoord.z >= Depth)
        {
            return 0;
        }
        else
        {
            return GetCell(neighboorCoord.x, neighboorCoord.z);
        }
    }

    struct DataCorordinate 
    {
        public int x;
        public int y;
        public int z;

        public DataCorordinate(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
    DataCorordinate[] offsets =
    {
        new DataCorordinate (0,0,1),
        new DataCorordinate (1,0,0),
        new DataCorordinate (0,0,-1),
        new DataCorordinate (-1,0,0),
        new DataCorordinate (0,1,0),
        new DataCorordinate (0,-1,0)

    };
}

public enum Direction
{
    North,
    East,
    South,
    West,
    Up,
    Down
}
