using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRope : MonoBehaviour
{
    // Start is called before the first frame update

    //list of rope prefabs to be spawned
    public GameObject[] ropeObjects;
    public Rigidbody2D hook;
    public GameObject rope;
    public int numLinks = 5;

    public HingeJoint topSegment;

    void Start()
    {
        Rigidbody2D prevBod = hook;
        GameObject rope = new GameObject();
        rope.name = "Rope";
        //sets layer to rope
        rope.layer = 10; 
        rope.transform.parent = transform;
        int rand = Random.Range(0, ropeObjects.Length);
        for (int i = 0; i < numLinks; i++)
        {
            GameObject newSeg = Instantiate(ropeObjects[rand]);
            newSeg.transform.parent = transform;
            newSeg.transform.position = transform.position;
            HingeJoint2D hj = newSeg.GetComponent<HingeJoint2D>();
            hj.connectedBody = prevBod;

            prevBod = newSeg.GetComponent<Rigidbody2D>();

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
