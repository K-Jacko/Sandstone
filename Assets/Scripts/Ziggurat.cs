using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ziggurat : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject[] Alters;
    public GameObject Base;
    private bool isPlaced;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //Hold data on stages. If i dont use two different scripts ill have to make a state machine for this to.
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        var layerMask = 1 << 6;
        if (!isPlaced)
        {
            if (Physics.Raycast(new Vector3(0, 100, 0), Vector3.down, out hit, 1000, layerMask))
            {
                transform.position = hit.point;
                isPlaced = true;
                var player = StageDirector.Instance.Player.gameObject;
                player.transform.position = spawnPoint.transform.position;
            }
        }
        
    }
}
