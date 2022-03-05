using System.Collections;
using System.Collections.Generic;
using Unity.XR.Oculus;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private int _width;

    private int _height;
    private float _cellSize;
    private int[,] gridArray;

    public Grid(int width, int height, float cellSize, Color color)
    {
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;
        gridArray = new int[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                var newXpos = (_width / -2);
                var newYpos = -(_height / 2);
                Debug.DrawLine(GetWorldPosition((int)newXpos + x, (int)newYpos + y), GetWorldPosition((int)newXpos + x, (int)newYpos + y + 1), color,100f);
                Debug.DrawLine(GetWorldPosition((int)newXpos + x, (int)newYpos + y), GetWorldPosition((int)newXpos + x + 1, (int)newYpos + y), color,100f);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), color,100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(_width / -2 , _height / 2 ), GetWorldPosition(_width / 2, _height / 2),Color.red,100f);
        Debug.DrawLine(GetWorldPosition(_width / 2, _height / -2 ), GetWorldPosition(_width / 2, _height / 2),Color.red,100f);
    }

    public int GetWidth()
    {
        return _width;
    }

    public int GetHeigth()
    {
        return _height;
    }
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, 0,y) * _cellSize;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition.x / _cellSize);
        y = Mathf.FloorToInt(worldPosition.y / _cellSize);
    }
    
    public void SetValue(int x, int y, int value)
    {
        if(x >= 0 && y >= 0 && x < _width && y < _height)
            gridArray[x, y] = value;
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            return gridArray[x, y];
        }
        else
        {
            return 0;
        }
    }

    public int GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }
}
