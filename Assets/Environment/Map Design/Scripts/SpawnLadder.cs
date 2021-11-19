using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLadder : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] ladderObjects;
    public int ladderLength;

    void Start()
    {
        GameObject ladder = new GameObject();
        ladder.name = "Ladder";
        ladder.layer = 10; //sets layer to rope
        ladder.transform.parent = transform;
        int rand = Random.Range(0, ladderObjects.Length);
        for (int y = 0; y < ladderLength; y++)
        {
            Vector3 ladderSpawnPosition = transform.position;
            ladderSpawnPosition.y -= y;
            GameObject instance = Instantiate(ladderObjects[rand], ladderSpawnPosition, Quaternion.identity);
            instance.transform.parent = transform.parent;
            Destroy(gameObject);
            instance.layer = 12;
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
