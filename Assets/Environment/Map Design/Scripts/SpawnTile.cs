using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] tileObjects;
    public GameObject[] oreObjects;
    public int orePencentage;

    void Start()
    {
        //sets game objects layer to BuildingBlock
        transform.gameObject.layer = 9;
        int randBlockOrOre = Random.Range(0, 101);  //if 0 then 
        if (randBlockOrOre > orePencentage)
        {
            int rand = Random.Range(0, tileObjects.Length);
            GameObject instance = Instantiate(tileObjects[rand], transform.position, Quaternion.identity);
            instance.transform.parent = transform.parent;
            Destroy(gameObject);
            instance.layer = 9;
            instance.AddComponent<BoxCollider2D>();
            instance.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
        else
        {
            int rand = Random.Range(0, oreObjects.Length);
            GameObject instance = Instantiate(oreObjects[rand], transform.position, Quaternion.identity);
            instance.transform.parent = transform.parent;
            Destroy(gameObject);
            instance.layer = 9;
            instance.AddComponent<BoxCollider2D>();
            instance.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;




        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
