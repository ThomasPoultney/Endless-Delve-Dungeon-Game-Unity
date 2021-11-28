
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnRope : MonoBehaviour
{
    public Rigidbody2D hook;
    public GameObject[] ropeSegments;
    public int numLinks = 5;
    [SerializeField]
    private int startLimit = 15;
    [SerializeField]
    private int increasingLimitAmount = 5;
    private float finishedSpawningAt;
    public HingeJoint topSegment;
    private bool enabledRigidBodies;
    // Start is called before the first frame update
    void Start()
    {
        GenerateRope();
        finishedSpawningAt = Time.time;



    }
    /// <summary>
    /// resposible for spawning each rope segment of the rope
    /// </summary>
    private void GenerateRope()
    {
        //offsets position by 0.5f because pivot point of hinge is set to top
        Vector3 ropeSpawnPosition = transform.position + new Vector3(0,0.5f,0);
        hook.transform.position = ropeSpawnPosition;
        Rigidbody2D prevBod = hook;
        //chooses random ropePrefab to spawn
        int rand = Random.Range(0, ropeSegments.Length);

        //spawns each rope segment
        for (int i = 0; i < numLinks; i++)
        {
            GameObject newSeg = Instantiate(ropeSegments[rand]);
            newSeg.transform.parent = transform;
            newSeg.transform.position = ropeSpawnPosition;
            HingeJoint2D hj = newSeg.GetComponent<HingeJoint2D>();
            hj.connectedBody = prevBod;
            JointAngleLimits2D joints =  new JointAngleLimits2D();
            //gradually increase joint angle limits the further we move down rope
            joints.min = -(startLimit + (increasingLimitAmount * i));
            joints.max = startLimit + (increasingLimitAmount * i);
            hj.limits = joints;
            prevBod = newSeg.GetComponent<Rigidbody2D>();

        }
    }

    // Update is called once per frame
    void Update()
    {
        //reenables box colider of rope segments on after a delay to avoid physics breaking them before they have chance to spread out
        if (enabledRigidBodies != true && Time.time - finishedSpawningAt >= 5)
        {
            foreach(Transform child in transform)
            {
                if(child.GetComponent<RopeSegment>() != null)
                {
                    child.GetComponent<RopeSegment>().GetComponent<BoxCollider2D>().enabled = true;                  
                }
            }
          enabledRigidBodies = true;
        }


    }
}
