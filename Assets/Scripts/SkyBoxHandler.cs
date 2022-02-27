using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxHandler : MonoBehaviour
{
    public GameObject player;

    public GameObject Skybox;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Skybox.transform.position = player.transform.position;
    }
}
