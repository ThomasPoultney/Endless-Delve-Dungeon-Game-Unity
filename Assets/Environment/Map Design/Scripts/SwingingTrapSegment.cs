using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class which spawns a set length of a Swinging spike trap
/// </summary>
public class SwingingTrapSegment : MonoBehaviour
{

    public GameObject connectedAbove;
    public GameObject connectedBelow;
    private GameObject playerObject;
    private Player_Collisions playerScript;

    public bool isPlayerAttached = false;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.Find("Player");
        playerScript = (Player_Collisions)playerObject.GetComponent(typeof(Player_Collisions));

        connectedAbove = GetComponent<HingeJoint2D>().connectedBody.gameObject;
        SwingingTrapSegment aboveSegment = connectedAbove.GetComponent<SwingingTrapSegment>();

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerScript.takeDamage(-1, false, new Vector2(0, 0), 0);
        }
    }
}
