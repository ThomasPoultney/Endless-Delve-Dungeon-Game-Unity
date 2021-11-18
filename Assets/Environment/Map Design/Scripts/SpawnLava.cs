using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLava : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] lavaObjects;

    void Start()
    {

        int rand = Random.Range(0, lavaObjects.Length);
        GameObject instance = Instantiate(lavaObjects[rand], transform.position, Quaternion.identity);

        instance.transform.parent = transform.parent;
        Destroy(gameObject);


    }

    // Update is called once per frame
    void Update()
    {

    }
}
