
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnRope : MonoBehaviour
{
    public Rigidbody2D hook;
    public GameObject[] ropeSegments;
    public int numLinks = 5;


    public HingeJoint topSegment;
    // Start is called before the first frame update
    void Start()
    {
        GenerateRope();
    }

    private void GenerateRope()
    {
        Vector3 ropeSpawnPosition = transform.position + new Vector3(0, 0.5f,0) ;
        hook.transform.position = ropeSpawnPosition;
        Rigidbody2D prevBod = hook;

        int rand = Random.Range(0, ropeSegments.Length);
        for (int i = 0; i < numLinks; i++)
        {
            GameObject newSeg = Instantiate(ropeSegments[rand] , ropeSpawnPosition, Quaternion.identity);
            newSeg.transform.parent = transform;
            newSeg.transform.position = ropeSpawnPosition;
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
