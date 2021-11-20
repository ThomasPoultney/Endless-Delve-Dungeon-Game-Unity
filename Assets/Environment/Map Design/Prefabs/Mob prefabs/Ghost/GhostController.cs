using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{

    public float speed;
    private GameObject player;
    private bool hunting = false;
    public float pursuitDistance = 4f;
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
            } else
            {
                ChangeAnimationState("Ghost_Idle");
            }

        }




    }


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


    private void ChangeAnimationState(string newState)
    {
        if (newState == currentAnimationState) return;

        animator.Play(newState);

        currentAnimationState = newState;
    }

}
