using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ziggurat : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject[] Alters;
    public GameObject Base;
    private bool isPlaced;
    private GameObject player;
    private LayerMask layerMask;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //player = StageDirector.Instance.Player.gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaced)
        {
            RaycastHit hit;
            layerMask = 1 << 6;
            if (Physics.Raycast(new Vector3(0, 100, 0), Vector3.down, out hit, 1000, layerMask))
            {
                transform.position = hit.point;
                isPlaced = true;
                player.transform.position = spawnPoint.transform.position;
            }
        }
    }
}
