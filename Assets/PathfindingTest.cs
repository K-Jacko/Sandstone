using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingTest : MonoBehaviour
{
    public GameObject pathFrom;
    public GameObject pathTo;

    private Pathfinding pathfinding;
    // Start is called before the first frame update
    void Start()
    {
        pathfinding = new Pathfinding(10, 10);
        pathfinding.GetGrid().GetXY(pathTo.transform.position, out int x, out int y);
        //pathfinding.GetGrid().GetXY(pathFrom.transform.position, out b, out b);
        List<PathNode> path = pathfinding.FindPath(0, 0, x, y);
        if (path != null)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(new Vector3(path[i].x,0, path[i].y) * 10f + new Vector3(1,0,1)* 5f - new Vector3(10 * 10 / 2,0,10 * 10 / 2 ), new Vector3(path[i+1].x,0, path[i+1].y) * 10f + new Vector3(1,0,1) * 5f - new Vector3(10 * 10 / 2,0,10 * 10 / 2 ), Color.magenta,100f);
                Debug.Log($"{new Vector3(path[i].x,0, path[i].y)}");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
