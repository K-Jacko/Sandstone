using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    public List<T> Create<T>(GameObject prefab, int count, GameObject parent)
        where T : MonoBehaviour
    {
        //New List
        List<T> newPool = new List<T>();

        //Create
        for(int i = 0; i < count; i++)
        {
            GameObject projectileObject = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, parent.transform);
            projectileObject.transform.localScale = new Vector3(1,1,1);
            T newProjectile = projectileObject.GetComponent<T>();

            newPool.Add(newProjectile);
        }

        return newPool;
    }
}
