using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRope : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] ropeObjects;
    public int ropeLength;

    void Start()
    {
        GameObject rope = new GameObject();
        rope.name = "Rope";
        rope.layer = 10; //sets layer to rope
        rope.transform.parent = transform;
        int rand = Random.Range(0, ropeObjects.Length);
        for (int y = 0; y < ropeLength; y++)
        {
            Vector3 ropeSpawnPosition = transform.position;
            ropeSpawnPosition.y -= y;
            GameObject instance = Instantiate(ropeObjects[rand], ropeSpawnPosition, Quaternion.identity);
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
