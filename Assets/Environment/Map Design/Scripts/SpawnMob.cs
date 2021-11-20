using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMob : MonoBehaviour
{
    //array of mob prefabs
    public GameObject[] mobs;
    // Start is called before the first frame update
    void Start()
    {
        //selects a random prefab to spawn
        int rand = Random.Range(0, mobs.Length);
        GameObject instance = Instantiate(mobs[rand], transform.position, Quaternion.identity);
        //destorys spawn point 
        Destroy(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
