using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public Transform origin;
    public GameObject zig;

    public GameObject player;

    private Transform spawn;

    public float StructureOffset = -1f;
    // Start is called before the first frame update
    void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(origin.position, origin.up * -1, out hit))
        {
            GameObject go = Instantiate(zig, hit.transform.position, hit.transform.rotation);
            spawn = go.transform.Find("Spawn");
            var transformPosition = go.transform.position;
            transformPosition.y = StructureOffset;
            go.transform.position = transformPosition;
            player.transform.position = spawn.transform.position;
        }
    }
}
