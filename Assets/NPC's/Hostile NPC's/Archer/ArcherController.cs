using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : MonoBehaviour
{
    public float walkSpeed = 0.2f;
    public float maxShootDistance = 10f;
    public Animator animator;
    private bool isAttacking = false;

    private Vector3 rayPosition;
    private bool movingRight = true;
    private string currentAnimationState;
    public LayerMask playerLayer;
    public LayerMask layersThatCanBeWalkedOn;
    public LayerMask layersThatCantBeWalkedThrough;
    private Vector2 playerCheckDirection;


    // Start is called before the first frame update
    void Start()
    {
    }


    void FixedUpdate()
    {
     
        RaycastHit2D playerInShootingRange = Physics2D.Raycast(transform.position, playerCheckDirection, maxShootDistance, playerLayer);


        if (playerInShootingRange.collider)
        {

            ChangeAnimationState("AA_Shoot");
           

        } else
        {
            ChangeAnimationState("AA_Idle");
        }
        
    }


    private void ChangeAnimationState(string newState)
    {
        if (newState == currentAnimationState) return;

        animator.Play(newState);

        currentAnimationState = newState;
    }
}
