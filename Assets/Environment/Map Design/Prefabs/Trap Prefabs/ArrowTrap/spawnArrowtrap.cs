using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnArrowtrap : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] tileObjects;
    public GameObject[] trapObjects;

    public PhysicsMaterial2D noFriction;

    [SerializeField]
    public int trapChance;

    void Start()
    {
        int blockChance = Random.Range(0, 101);
        //print(blockChance + " and trap chance is " + trapChance);
        // If bigger than trap chance, then make it a normal block tile.
        if (blockChance > trapChance)
        {
            print("Making a normal block!");
            int rand = Random.Range(0, tileObjects.Length);
            GameObject instance = Instantiate(tileObjects[rand], transform.position + new Vector3(0, 0, 100), Quaternion.identity);
            instance.transform.parent = transform.parent;
            Destroy(gameObject);
            instance.layer = 9;
            instance.AddComponent<BoxCollider2D>();
            instance.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            instance.GetComponent<Rigidbody2D>().sharedMaterial = noFriction;
            instance.GetComponent<BoxCollider2D>().sharedMaterial = noFriction;

        }
        // Else make it a trap.
        else
        {
            int rand = Random.Range(0, trapObjects.Length);
            //Clones arrowtrap prefab and spawns it at current Pos.
            GameObject instance = Instantiate(trapObjects[rand], transform.position + new Vector3(0, 0, 100), Quaternion.identity);

            instance.transform.parent = transform.parent;
            //sets layer to Arrow Trap
            instance.layer = 17;
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
