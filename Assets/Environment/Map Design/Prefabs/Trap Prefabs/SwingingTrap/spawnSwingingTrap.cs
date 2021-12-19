
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script responsible for spawning a length of spike ball traps and setting its physics constraints.
/// </summary>
public class spawnSwingingTrap : MonoBehaviour
{
    public Rigidbody2D hook;
    public GameObject[] ropeSegments;
    public int numLinks;
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
        // Move the spawn position up.
        Vector3 ropeSpawnPosition = transform.position + new Vector3(0,0.55f,0);
        hook.transform.position = ropeSpawnPosition;
        Rigidbody2D prevBod = hook;

        // Randomise length of rope.

        numLinks = Random.Range(3, 6);

        // Spawns each rope segment
        for (int i = 0; i < numLinks; i++)
        {
            GameObject newSeg = Instantiate(ropeSegments[0]);
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
        if (enabledRigidBodies != true && Time.time - finishedSpawningAt >= 2)
        {
            foreach(Transform child in transform)
            {
                if(child.GetComponent<SwingingTrapSegment>() != null)
                {
                    print("reenabling box colllider");
                    child.GetComponent<SwingingTrapSegment>().GetComponent<BoxCollider2D>().enabled = true;                  
                }
            }
          enabledRigidBodies = true;
        }
    }
}
