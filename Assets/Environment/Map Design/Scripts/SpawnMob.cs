using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMob : MonoBehaviour
{
    public GameObject[] mobs;
    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, mobs.Length);
        GameObject instance = Instantiate(mobs[rand], transform.position, Quaternion.identity);
        Destroy(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
