using UnityEngine;

public class SkelentonMobController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 0.2f;
    [SerializeField]
    private float attackDistance = 0.5f;
    public Animator animator;
    private bool isAttacking = false;

    private Vector3 rayPosition;
    private bool movingRight = true;
    private string currentAnimationState = "Skelenton_Idle";
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private LayerMask layersThatCanBeWalkedOn;
    [SerializeField]
    private LayerMask layersThatCantBeWalkedThrough;
    private Vector2 playerCheckDirection;
    private BoxCollider2D boxCollider;


    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }


    void FixedUpdate()
    {
        
        RaycastHit2D isOnPlatformEdge;
        RaycastHit2D blockInFront;
        Vector3 movingRotation;

        //ray to check if player is withing attack range
        RaycastHit2D playerInMeleeRange = Physics2D.Raycast(transform.position, playerCheckDirection, attackDistance, playerLayer);


        if (playerInMeleeRange.collider)
        {
            
            ChangeAnimationState("Skelenton_Swing");
            boxCollider.size = new Vector2(2, 1);
            isAttacking = true;

        }
        else
        {
            boxCollider.size = new Vector2(1, 1);

            if (movingRight)
            {
                transform.position += new Vector3(+walkSpeed, 0, 0);
                //sets the direction object is facing to right
                movingRotation = new Vector3(0, -180, 0);
                //checks if we are at platform end
                isOnPlatformEdge = Physics2D.Raycast(transform.position + new Vector3(0.1f, 0, 0), Vector2.down, 1f,layersThatCanBeWalkedOn);
                //checks if we can move forward
                blockInFront = Physics2D.Raycast(transform.position + new Vector3(0.1f, 0, 0), playerCheckDirection, 0.2f, layersThatCantBeWalkedThrough);
                ChangeAnimationState("Skelenton_Walk");
                playerCheckDirection = Vector2.right;

            }
            else
            {
                transform.position += new Vector3(-walkSpeed, 0, 0);
                //sets the direction the object is facing to left
                movingRotation = new Vector3(0, 0, 0);
                //checks if we are at platform end
                isOnPlatformEdge = Physics2D.Raycast(transform.position + new Vector3(-0.1f, 0, 0), Vector2.down, 1f,layersThatCanBeWalkedOn) ;
                //checks if we can move forward
                blockInFront = Physics2D.Raycast(transform.position + new Vector3(0.1f, 0, 0), playerCheckDirection, 0.2f, layersThatCantBeWalkedThrough);

                ChangeAnimationState("Skelenton_Walk");
                playerCheckDirection = Vector2.left;
            }

            if (isOnPlatformEdge.collider == false || blockInFront.collider == true)
            {
                movingRight = !movingRight;
                transform.eulerAngles = movingRotation;

            }
        }
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