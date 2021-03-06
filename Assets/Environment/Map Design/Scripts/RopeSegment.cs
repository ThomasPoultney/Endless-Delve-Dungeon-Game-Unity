using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class which stores information about the attached rope segment. 
/// 
/// It stores the rope segment that is placed above and below the attached one.
/// </summary>
public class RopeSegment : MonoBehaviour
{

    public GameObject connectedAbove;
    public GameObject connectedBelow;

    public bool isPlayerAttached = false;

    // Start is called before the first frame update
    void Start()
    {
        connectedAbove = GetComponent<HingeJoint2D>().connectedBody.gameObject;
        RopeSegment aboveSegment = connectedAbove.GetComponent<RopeSegment>();

       if (aboveSegment != null)
        {
            aboveSegment.connectedBelow = gameObject;
            GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, -1);

        } else
        {
            GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0,0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
