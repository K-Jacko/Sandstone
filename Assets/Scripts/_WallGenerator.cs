using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

public class _WallGenerator : MonoBehaviour
{
    WinCintrols controls;
    public GameObject brick;
    public int brickNumber = 5;
    public float zSpace = 1;    
    
    void Awake()
    {
        controls = new WinCintrols();
        controls.Player.Generate.performed += ctx => Generate();
    }
    void Start()
    {
        
    }
    
    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void Generate()
    {
        Vector3 origin = new Vector3(0,0,0);
        for(int i = 0; i < brickNumber; i++)
        {   
            Instantiate(brick,origin, new Quaternion(0,0,0,0));
            origin += new Vector3(0,0,zSpace);
        }
    }
     
}
