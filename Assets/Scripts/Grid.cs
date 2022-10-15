using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.Oculus;
using UnityEngine;

public class Grid<TGridObject>
{
    public event EventHandler<OnGridValueChangedEventArgs> onGridValueChanged;

    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }
    private int _width;
    private int _height;
    private float _cellSize;
    private Vector3 _originPosition;
    private TGridObject[,] gridArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Color color, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject,bool debugGrid)
    {
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;
        this._originPosition = originPosition - new Vector3(_width * _cellSize / 2,0,_height * _cellSize /2 );
        gridArray = new TGridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                if (debugGrid)
                {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition( x,y + 1), color,100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1,  y), color,100f); 
                }
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), color,100f);
            }
        }
        //TODO : Draw outer Rim
         Debug.DrawLine(GetWorldPosition((int)originPosition.x,(int)originPosition.y + _height), GetWorldPosition((int)originPosition.x + _width, (int)originPosition.y + _height),color,100f);
         Debug.DrawLine(GetWorldPosition((int)originPosition.x + _width, (int)originPosition.y ), GetWorldPosition((int)originPosition.x + _width, (int)originPosition.y + _height),color,100f);
         //Debug.DrawLine(GetWorldPosition((int)originPosition.x,(int)originPosition.y - _height) , GetWorldPosition((int)originPosition.x + _width, (int)originPosition.y ),Color.red,100f);
        
    }

    public int GetWidth()
    {
        return _width;
    }

    public int GetHeigth()
    {
        return _height;
    }
    
    public float GetCellSize() {
        return _cellSize;
    }

    public Vector3 GetOffset()
    {
        return _originPosition;
    }
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, 0,y) * _cellSize + _originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
        y = Mathf.FloorToInt((worldPosition - _originPosition).z / _cellSize);
    }
    
    public void SetGridObject(int x, int y, TGridObject value)
    {
        if(x >= 0 && y >= 0 && x < _width && y < _height)
            gridArray[x, y] = value;
        onGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs {x = x, y = y});
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
        if (onGridValueChanged != null) onGridValueChanged(this, new OnGridValueChangedEventArgs {x = x, y = y});
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }
}
