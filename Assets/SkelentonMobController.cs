using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelentonMobController : MonoBehaviour
{
    public float walkSpeed = 20f;
    public float horizontalMove = 0f;
    
    public Animator animator;
    private bool isAttacking = false;

    private Vector3 rayPosition;
    private bool movingRight = true;
    private string currentAnimationState = "Skelenton_Idle";
    public LayerMask playerLayer;
    private Vector2 playerCheckDirection;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D isOnPlatformEdge;
        Vector3 movingRotation;
        

        RaycastHit2D playerInMeleeRange = Physics2D.Raycast(transform.position, playerCheckDirection, 1f, playerLayer);
        

        if (playerInMeleeRange.collider)
        {
            ChangeAnimationState("Skelenton_Swing");
            isAttacking = true;

        }
        else
        {
            if (movingRight)
            {
                transform.position += new Vector3(+walkSpeed, 0, 0);
                movingRotation = new Vector3(0, -180, 0);
                isOnPlatformEdge = Physics2D.Raycast(transform.position + new Vector3(1, 0, 0), Vector2.down, 2f);
                ChangeAnimationState("Skelenton_Walk");
                playerCheckDirection = Vector2.right;

            }
            else
            {
               transform.position += new Vector3(-walkSpeed, 0, 0);
                movingRotation = new Vector3(0, 0, 0);
                isOnPlatformEdge = Physics2D.Raycast(transform.position + new Vector3(-1, 0, 0), Vector2.down, 2f);
                ChangeAnimationState("Skelenton_Walk");
                playerCheckDirection = Vector2.left;
            }

            if (isOnPlatformEdge.collider == false)
            {
                movingRight = !movingRight;
                transform.eulerAngles = movingRotation;

            }
        }
    }


    private void ChangeAnimationState(string newState)
    {
        if (newState == currentAnimationState) return;

        animator.Play(newState);

        currentAnimationState = newState;
    }
}
