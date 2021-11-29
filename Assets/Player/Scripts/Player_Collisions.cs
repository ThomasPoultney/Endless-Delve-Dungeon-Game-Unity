using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Collisions : MonoBehaviour
{
    public int health = 2;
    public bool canBeDazed;
    public bool canDie;
    public bool canBeKnockedDown;
    public bool canTakeDamage = true;
    [HideInInspector]
    public bool isDazed = false;
    [HideInInspector]
    public bool isDieing = false;
    [HideInInspector]
    public bool isKnockedBack = false;


    private float timeKnockedBack = 0f;
    private float timeSinceKnockBack = 0f;

    private float timeDazed = 0f;
    private float timeSinceDazed = 0f;

    private float timeDieing = 0f;
    private float timeSinceDieing = 0f;
    public bool isAlive = true;

    [SerializeField] private float interactRadius = 0.4f;


    [SerializeField] private float iFrameTime = 0.3f;
    private float timeSinceLastDamage = 0f;
    private bool invicible = false;

    [SerializeField] private AnimationClip dazedAnimation = null;
    [SerializeField] private AnimationClip deadAnimation = null;
    [SerializeField] private AnimationClip knockedBackAnimation = null;
    [SerializeField] private AudioSource takeDamageSound = null;
    [SerializeField] private AudioSource deathSound = null;

    [SerializeField] private GameObject bloodSplatter = null;

    [SerializeField] private LayerMask interactionLayer;

    [SerializeField] private Transform headPos;
    [SerializeField] private Transform feetPos;



    public void takeDamage(int amount, bool doesKnockBack, Vector2 knockBacKDirection, float knockBackForce)
    {
        if (!canTakeDamage || invicible || !isAlive)
        {
            return;
        }

        Player_Controller Player_Controller = transform.GetComponent<Player_Controller>();
        
        if (transform.GetComponent<PlayerRopeController>().attached == true)
        {
            transform.GetComponent<PlayerRopeController>().Detach(true);
        }

        Player_Controller.isWallGrabbing = false;
        Player_Controller.isWallSliding = false;
        Player_Controller.canWallGrab = false;
        Player_Controller.wallGrabResetTimer = 0.4f;
        Player_Controller.timeSinceCannotWallGrab = Time.time;


        health += amount;
        invicible = true;
        timeSinceLastDamage = Time.time;
        AnimationController animationController = transform.GetComponent<AnimationController>();


        if (health <= 0 && canDie)
        {

            setPlayerDead();
            
        }
        else if (health > 0 && canBeDazed && !doesKnockBack)
        {
            isDazed = true;
            Rigidbody2D playerBody = transform.GetComponent<Rigidbody2D>();
            playerBody.velocity = Vector2.zero;
            animationController.ChangeAnimationState(dazedAnimation.name);
            timeDazed = animationController.getCurrentAnimationLength();
            timeSinceDazed = Time.time;
            if (takeDamageSound != null)
            {

            }
        }
        else if (health > 0 && doesKnockBack && canBeKnockedDown)
        {
            isKnockedBack = true;
            Rigidbody2D playerBody = transform.GetComponent<Rigidbody2D>();
            playerBody.velocity = knockBacKDirection * knockBackForce;
            
            animationController.ChangeAnimationState(knockedBackAnimation.name);
            timeKnockedBack = animationController.getCurrentAnimationLength();
            timeSinceKnockBack = Time.time;
        }

      
        Instantiate(bloodSplatter,transform);
        
    }

    private void setPlayerDead()
    {
        isDieing = true;
        isAlive = false;
        AnimationController animationController = transform.GetComponent<AnimationController>();
        animationController.ChangeAnimationState(deadAnimation.name);
        timeDieing = animationController.getCurrentAnimationLength();
        Debug.Log("Dead");
        timeSinceDieing = Time.time;
        if (deathSound != null)
        {

        }
    }

    public void Update()
    {
        if (isDazed && Time.time - timeSinceDazed > timeDazed)
        {
            isDazed = false;
        }

        if (isDieing && Time.time - timeSinceDieing > timeDieing)
        {
            //GameObject.Destroy(gameObject);
            isDieing = false;
        }

        if (isKnockedBack && Time.time - timeSinceKnockBack > timeKnockedBack)
        {
            isKnockedBack = false;
        }

        if(Time.time - timeSinceLastDamage > iFrameTime)
        {
            invicible = false;
        }


        Vector2 overlapPointOne = new Vector2(headPos.position.x + interactRadius, headPos.position.y);
        Vector2 overlapPointTwo = new Vector2(feetPos.position.x - interactRadius, feetPos.position.y);
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(feetPos.position, interactRadius,interactionLayer);

        if(objectsInRange.Length > 0) 
        {
            foreach(Collider2D obj in objectsInRange)
            {
                if (obj.gameObject.layer == 11) //loot
                {
                    Destroy(obj.gameObject);
                }

                if (obj.gameObject.layer == 12) //ladder
                {
                    bool touchingLadder = true;
                    Debug.Log("Touching Ladder");
                }

                if (obj.gameObject.layer == 7) //spikes
                {

                    takeDamage(-1, false, Vector2.zero, 0);
                }

                if (obj.gameObject.layer == 13) //lava
                {
                    setPlayerDead();                   
                }


            }
        }

    }


    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(feetPos.position, interactRadius);
    }

}
