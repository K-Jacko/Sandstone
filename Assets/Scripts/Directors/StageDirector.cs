
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class StageDirector : MonoBehaviour
{
    public static StageDirector Instance { get; private set; }
    public List<Entity> friendlyEntities;

    public int gridSize;
    public int cellSize;
    
    public int wallet;
    
    public float markerRadius;

    public static MarkerGenerator markerGenerator { get; private set; }

    // This wil be a script to spawn all the interactables and objectives on the map 
    // This will use a wallet with a set ammount to do so 
    // This will spawn these objectives via scriptable objects
    // This will happen while player is in load 
    // This will go in sequence along all cells on grid and take from the list of raycastYFloats 
    void Start()
    {
        Instance = this;
        HandleFriendlies();
        GenerateMarkers();
        GenerateCombatDirector();
        //dd
    }

    void HandleFriendlies()
    {
        //TODO Sort friendly entities
        friendlyEntities = new List<Entity>();
        friendlyEntities.Add(SceneDirector.Instance.Player);
    }

    void GenerateMarkers()
    {
        markerGenerator = gameObject.AddComponent<MarkerGenerator>();
        markerGenerator.Init(gridSize,cellSize,GenerateNewProps);
    }

    void GenerateNewProps()
    {
        var propGenerator = gameObject.AddComponent<PropGenerator>();

        for (int i = 0; i < gridSize * (SceneDirector.Instance.coEef + 1) ; i++)
        {
            propGenerator.GeneratePropOnGrid(markerGenerator.SpawnNodeGrid().GetGridObject(markerGenerator.RaycastLocations()[Random.Range(0, markerGenerator.RaycastLocations().Length)]), wallet,markerRadius);
        }
    }

    private void GenerateCombatDirector()
    {
        var combatDirector = gameObject.AddComponent<CombatDirector>();
        combatDirector.Init();
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < markerGenerator.RaycastLocations().Length; i++)
        {
            Gizmos.DrawWireSphere(markerGenerator.RaycastLocations()[i],markerRadius);
        }
    }
}
