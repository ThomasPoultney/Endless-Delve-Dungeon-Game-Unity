using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns a random lava object and sets its layer, and adds a particle system.
/// </summary>
public class SpawnLava : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] lavaObjects;

    void Start()
    {

        int rand = Random.Range(0, lavaObjects.Length);
        //Clones lava prefab and spawns it at current Pos.
        GameObject instance = Instantiate(lavaObjects[rand], transform.position + new Vector3(0,0,-2), Quaternion.identity);

        instance.transform.parent = transform.parent;
        //sets layer to lava
        instance.layer = 13;
      
        //sets it so you can move through lava
        Destroy(gameObject);


    }

    // Update is called once per frame
    void Update()
    {

    }
}
