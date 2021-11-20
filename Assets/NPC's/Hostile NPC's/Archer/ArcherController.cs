using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 0.2f;
    [SerializeField]
    private float maxShootDistance = 10f;
    public Animator animator;
    private bool isAttacking = false;
    private Vector3 rayPosition;
    private bool movingRight = true;
    private string currentAnimationState;
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private LayerMask layersThatCanBeWalkedOn;
    [SerializeField]
    private LayerMask layersThatCantBeWalkedThrough;
    private Vector2 playerCheckDirection;


    // Start is called before the first frame update
    void Start()
    {
    }


    void FixedUpdate()
    {
        //checks if there is a player in range
        RaycastHit2D playerInShootingRange = Physics2D.Raycast(transform.position, playerCheckDirection, maxShootDistance, playerLayer);


        //if there is a player in range we shoot
        if (playerInShootingRange.collider)
        {
            shootArrow();
        } else
        {
            ChangeAnimationState("AA_Idle");
        }
        
    }

    /// <summary>
    /// Creates a arrow to be fired from the archer.
    /// </summary>
    private void shootArrow()
    {
        ChangeAnimationState("AA_Shoot");
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
