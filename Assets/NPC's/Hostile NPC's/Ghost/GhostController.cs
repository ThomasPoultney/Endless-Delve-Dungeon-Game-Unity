using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private GameObject player;
    private bool hunting = false;
    [SerializeField]
    private float pursuitDistance = 4f;
    public Animator animator; 
    private String currentAnimationState;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {

            if (Vector2.Distance(transform.position, player.transform.position) < pursuitDistance)
            {
                Hunt();
                hunting = true;
            } else
            {
                ChangeAnimationState("Ghost_Idle");
                hunting=false;
            }

        }




    }

    /// <summary>
    /// Moves the ghost towards the player, sets skull to face the players
    /// </summary>
    private void Hunt()
    {
        ChangeAnimationState("Ghost_Shriek");
        
        if (transform.position.x < player.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }


    /// <summary>
    /// Changes the animation state of the animator attached to the object
    /// </summary>
    /// <param name="newState"></param>
    private void ChangeAnimationState(string newState)
    {
        if (newState == currentAnimationState) return;

        animator.Play(newState);

        currentAnimationState = newState;
    }

}