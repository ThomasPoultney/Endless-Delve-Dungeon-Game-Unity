using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLadder : MonoBehaviour
{
    //list of ladder prefabs to that can be spawned
    public GameObject[] ladderObjects;
    public int ladderLength;


    // Start is called before the first frame update
    void Start()
    {

        GameObject ladder = new GameObject();
        ladder.name = "Ladder";
        ladder.layer = 12; //sets layer to ladder
        ladder.transform.parent = transform; 
        int rand = Random.Range(0, ladderObjects.Length); //select a random ladder prefab.
        for (int y = 0; y < ladderLength; y++)
        {
            Vector3 ladderSpawnPosition = transform.position;
            ladderSpawnPosition.y -= y;
            GameObject instance = Instantiate(ladderObjects[rand], ladderSpawnPosition, Quaternion.identity);
            instance.transform.parent = transform.parent;
            Destroy(gameObject);
            instance.layer = 12; //sets layer to ladder
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
