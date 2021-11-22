using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRopeTest : MonoBehaviour
{
    public Rigidbody2D rb;
    private HingeJoint2D hj;

    private Image countdownImage;

    public float pushForce = 10f;

    public bool attached = false;

    private Transform ropeAttachedTo;
    private Transform ropeSegmentAttachedTo;
    private GameObject disregard; //stops player attaching to the same rope again
    public float timeBeforePlayerCanAttachToSameRope = 1f;

    private float lastDetachTime;


    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        hj = gameObject.GetComponent<HingeJoint2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (ropeAttachedTo !=null)
        {
          
            HingeJoint2D joint = transform.GetComponent<HingeJoint2D>();
            float offsetX = 0;
            float offsetY = joint.connectedAnchor.y;
            joint.connectedAnchor.Set(offsetX, offsetY);
        }
     

        if (Time.time - lastDetachTime > timeBeforePlayerCanAttachToSameRope)
        {
            disregard = null;
        }
        CheckKeyBoardInputs();
      
    }

    private void CheckKeyBoardInputs()
    {
     

        if (Input.GetKeyDown(KeyCode.Space) && attached)
        {
            Detach();
        }

        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            if(attached)
            {
                rb.AddRelativeForce(new Vector3(-1, 0, 0) * pushForce);
            }
        }

        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            if (attached)
            {
                rb.AddRelativeForce(new Vector3(1, 0, 0) * pushForce);
            }
        }

        if(Input.GetKey("w") || Input.GetKey("up") && attached)
        {
            Slide(1);
        }

        if (Input.GetKey("s") || Input.GetKey("down") && attached)
        {
            Slide(-1);
        }

      
    }

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
        ropeSegment.gameObject.GetComponent<RopeSegment>().isPlayerAttached = true;
        ropeSegmentAttachedTo = ropeSegment.transform.parent;
        hj.connectedBody = ropeSegment;
        hj.enabled = true;
        attached = true;
        ropeAttachedTo = ropeSegment.gameObject.transform.parent;
    }

    private void Detach()
    {
        hj.connectedBody.gameObject.GetComponent<RopeSegment>().isPlayerAttached = true;
        disregard = hj.connectedBody.gameObject.GetComponent<RopeSegment>().transform.parent.gameObject;
        ropeAttachedTo = null;
        attached = false;
        hj.enabled = false;
        hj.connectedBody = null;
        lastDetachTime = Time.time;
    }

   

    private void Slide(int amountToSlide)
    {
        if (amountToSlide > 0)
        {

        } else if(amountToSlide < 0)
        {

        }
    }
}




