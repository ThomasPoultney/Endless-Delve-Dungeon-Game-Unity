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
        GameObject instance = Instantiate(lavaObjects[rand], transform.position + new Vector3(0,0,100), Quaternion.identity);

        instance.transform.parent = transform.parent;
        instance.layer = 13;
        instance.AddComponent<BoxCollider2D>();
        instance.GetComponent<BoxCollider2D>().isTrigger = true;
        Destroy(gameObject);


    }

    // Update is called once per frame
    void Update()
    {

    }
}
