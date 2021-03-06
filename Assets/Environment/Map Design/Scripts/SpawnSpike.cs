using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns a random spike prefabs and sets up it layers. 
/// </summary>
public class SpawnSpike : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] spikeObjects;

    void Start()
    {
        int rand = Random.Range(0, spikeObjects.Length);
        GameObject instance = Instantiate(spikeObjects[rand], transform.position + new Vector3(0, 0, 100) , Quaternion.identity);
        instance.transform.parent = transform.parent;
        Destroy(gameObject);


        transform.gameObject.layer = 7; //Sets layer to spike
        instance.layer = 7; //Sets layer to spike


    }

    // Update is called once per frame
    void Update()
    {

    }
}
