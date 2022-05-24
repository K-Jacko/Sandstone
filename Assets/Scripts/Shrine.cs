using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour
{
    private bool isPlaced;
    private LayerMask layerMask;
    void Start()
    {
        layerMask = 1 << 6;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaced)
        {
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(transform.position.x, 100, transform.position.z), Vector3.down, out hit, 1000, layerMask))
            {
                transform.position = hit.point;
                isPlaced = true;
            }
        }
    }
}
