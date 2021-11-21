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
    private GameObject disregard; //stops player attaching to the same rope segment again
    public float timeBeforePlayerCanAttachToSameRope = 1f;



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
        CheckKeyBoardInputs();
    }

    private void CheckKeyBoardInputs()
    {
        if(Input.GetKey("a") || Input.GetKey("left"))
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

        if(Input.GetKeyDown(KeyCode.Space) && attached)
        {
            Detach();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!attached) {
            //
            if(collision.gameObject.tag == "Rope")
            {
                if(ropeAttachedTo != collision.gameObject.transform.parent) {
                    if (disregard == null || collision.gameObject.transform.parent != disregard)
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
        hj.connectedBody = ropeSegment;
        hj.enabled = true;
        attached = true;
        ropeAttachedTo = ropeSegment.gameObject.transform.parent;
    }

    private void Detach()
    {
        hj.connectedBody.gameObject.GetComponent<RopeSegment>().isPlayerAttached = true;
        disregard = hj.connectedBody.gameObject.GetComponent<RopeSegment>().transform.parent.gameObject;
        attached = false;
        hj.enabled = false;
        hj.connectedBody = null;


        StartCoroutine(setDisregard());
     
    }

    IEnumerator setDisregard()
    {

        yield return new WaitForSeconds(timeBeforePlayerCanAttachToSameRope);
        ropeAttachedTo = null;


    }

    private void Slide(int amountToSlide)
    {
        RopeSegment myConnection = hj.connectedBody.gameObject.GetComponent<RopeSegment>();
        GameObject newSeg = null;

        if(amountToSlide > 0)
        {
            //checks if there is a conneciton abouve
            if(myConnection.connectedAbove != null)
            {
                //checks that the 
                if(myConnection.connectedAbove.gameObject.GetComponent<RopeSegment>() != null)
                {
                    newSeg = myConnection.connectedAbove;
                }
            }
        } else
        {
            if (myConnection.connectedBelow != null)
            {
                newSeg = myConnection.connectedBelow;
            }
        }

        if(newSeg != null)
        {
          
        }
    }
}




