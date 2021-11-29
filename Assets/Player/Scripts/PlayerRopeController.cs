using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRopeController : MonoBehaviour
{
    public Rigidbody2D playerBody; //the players rigidbody
    private HingeJoint2D playerHinge; //hinge joint connected to player

    private Image countdownImage;

    public float pushForce = 10f;

    public bool attached = false;
    public float playerMass = 1;

    private Transform ropeAttachedTo;
    private Transform ropeSegmentAttachedTo;
    private GameObject disregard; //stops player attaching to the same rope again
    public float timeBeforePlayerCanAttachToSameRope = 1f;
    private float lastDetachTime;

    private float ropeClimbStartValue; //current rope segment position used for lerping
    private float ropeClimbEndValue; //next rope segment position used for lerping
    private float climbTime = 0.3f;
    private bool climbing = false;
    private float valueToLerp; //stores the current lerping value

    private float timeElapsed;
    


    private void Awake()
    {
        playerBody = gameObject.GetComponent<Rigidbody2D>();
        playerHinge = gameObject.GetComponent<HingeJoint2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

   
    void Update()
    {
        //lerps player from one rope segment to the next so the animation is smoother.
        if (climbing == true && timeElapsed < climbTime)
        {
            valueToLerp = Mathf.Lerp(ropeClimbStartValue, ropeClimbEndValue, timeElapsed / climbTime);
            transform.position = new Vector2(transform.position.x, valueToLerp );
            timeElapsed += Time.deltaTime;
        } else if(climbing == true)
        {
            climbing = false;
        }
     
        CheckKeyBoardInputs();

        if (ropeAttachedTo !=null)
        {        
            HingeJoint2D joint = transform.GetComponent<HingeJoint2D>();
            transform.rotation = ropeSegmentAttachedTo.transform.rotation;
            float offsetX = 0;
            float offsetY = joint.connectedAnchor.y;
            joint.connectedAnchor.Set(offsetX, offsetY);
        }
     
        //reenables box colliders on ropes after reset timer
        if(disregard != null)
        {
            if (Time.time - lastDetachTime > timeBeforePlayerCanAttachToSameRope)
            {
                foreach (Transform child in disregard.transform)
                {
                    if (child.GetComponent<RopeSegment>() != null)
                    {
                        child.GetComponent<BoxCollider2D>().isTrigger = false;
                    }
                }
                disregard = null;
            }

        }



    }
    /// <summary>
    /// Retrieves any player inputs and fires the relevant functions
    /// </summary>
    private void CheckKeyBoardInputs()
    {
     
        if(attached)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Detach(false);
            }

            if (Input.GetKey("a") || Input.GetKey("left"))
            {

                playerBody.AddRelativeForce(new Vector3(-1, 0, 0) * pushForce);

            }

            if (Input.GetKey("d") || Input.GetKey("right"))
            {

                playerBody.AddRelativeForce(new Vector3(1, 0, 0) * pushForce);

            }

            if ((Input.GetKeyDown("w") || Input.GetKeyDown("up")))
            {
                Slide(1);
            }

            if ((Input.GetKeyDown("s") || Input.GetKeyDown("down")))
            {
                Slide(-1);
            }
        }
       
    }

    /// <summary>
    /// Checks if player is colliding with a rope segment, if they are we attach to the rope segment.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {  
        
        if(!attached) {
            if(collision.gameObject.tag == "Rope")
            {
                if(ropeAttachedTo != collision.gameObject.transform.parent.gameObject) {
                    if (disregard == null || collision.gameObject.transform.parent.gameObject != disregard)
                    {                    
                        Attach(collision.gameObject.GetComponent<Rigidbody2D>());
                    }
                }
            }
        }
    }

    private void Attach(Rigidbody2D ropeSegment)
    {
        transform.GetComponent<Rigidbody2D>().mass = 0;
        ropeSegment.gameObject.GetComponent<RopeSegment>().isPlayerAttached = true;
        ropeSegmentAttachedTo = ropeSegment.transform.parent;
        playerHinge.connectedBody = ropeSegment;
        playerHinge.enabled = true;
        attached = true;
        ropeAttachedTo = ropeSegment.gameObject.transform.parent;
    }

    public void Detach(bool knockedOff)
    {

        transform.GetComponent<Rigidbody2D>().mass = playerMass;
        playerHinge.connectedBody.gameObject.GetComponent<RopeSegment>().isPlayerAttached = true;
        disregard = playerHinge.connectedBody.gameObject.GetComponent<RopeSegment>().transform.parent.gameObject;

        //disables colliders on ropesegments to stop player reattaching to them
        foreach (Transform child in ropeAttachedTo)
        {
            if (child.GetComponent<RopeSegment>() != null)
            {
                child.GetComponent<BoxCollider2D>().isTrigger = true;
            }
            
        }
        ropeAttachedTo = null;
        attached = false;
        playerHinge.connectedBody = null;
        playerHinge.enabled = false;     
        //timer to stop you attaching back to the same rope
        lastDetachTime = Time.time;
        //Tells player controller to jump when we detach from rope
        if(!knockedOff)
        {
            transform.GetComponent<Player_Controller>().Jump();
        }
    }

   
    /// <summary>
    /// Function repsposible for sliding player up or down a rope.
    /// </summary>
    /// <param name="amountToSlide"></param>
    private void Slide(int amountToSlide)
    {
        //current rope segment the player is attached to
        RopeSegment playerConnection = playerHinge.connectedBody.gameObject.GetComponent<RopeSegment>();
        GameObject newSeg = null;
        if (amountToSlide > 0)
        {
            //stops player sliding if there are no segments above to slide to
            if(playerConnection.connectedAbove != null)
            {
                if(playerConnection.connectedAbove.gameObject.GetComponent<RopeSegment>() != null)
                {
                    newSeg = playerConnection.connectedAbove;
                }
            }
        } 
        else 
        {
            if(playerConnection.connectedBelow != null)
            {
                newSeg = playerConnection.connectedBelow;
            }
        }

        //if we can slide
        if(newSeg != null)
        {
          
            //stores the position of the current segment and the position of the next segment for lerping
            ropeClimbStartValue = transform.position.y;
            ropeClimbEndValue = newSeg.transform.position.y;

            climbing = true;
            timeElapsed = 0; //resets climbing timer
            
            //sets player connected to next segment
            playerConnection.isPlayerAttached = false;
            newSeg.GetComponent<RopeSegment>().isPlayerAttached = true;
            playerHinge.connectedBody = newSeg.GetComponent<Rigidbody2D>();
        }
    }
}




