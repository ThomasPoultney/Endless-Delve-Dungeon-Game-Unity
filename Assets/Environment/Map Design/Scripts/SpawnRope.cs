using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRope : MonoBehaviour
{
    // Start is called before the first frame update

    //list of rope prefabs to be spawned
    public GameObject[] ropeObjects;
    //how many rope object to spawn
    public int ropeLength;

    void Start()
    {
        GameObject rope = new GameObject();
        rope.name = "Rope";
        //sets layer to rope
        rope.layer = 10; 
        rope.transform.parent = transform;
        int rand = Random.Range(0, ropeObjects.Length);
        //spawns length of rope
        for (int y = 0; y < ropeLength; y++)
        {
            Vector3 ropeSpawnPosition = transform.position;
            ropeSpawnPosition.y -= y;
            GameObject instance = Instantiate(ropeObjects[rand], ropeSpawnPosition + new Vector3(0,0,100), Quaternion.identity);
            instance.transform.parent = transform.parent;
            Destroy(gameObject);
            instance.layer = 10;
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
