using UnityEngine;

public class SlimeController : MonoBehaviour
{
    public float walkSpeed = 0.2f;
    public float attackDistance = 0.5f;
    public Animator animator;
    private bool isAttacking = false;

    private Vector3 rayPosition;
    private bool movingRight = false;
    private string currentAnimationState = "Slime_Idle";
    public LayerMask playerLayer;
    public LayerMask layersThatCanBeWalkedOn;
    public LayerMask layersThatCantBeWalkedThrough;
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


        RaycastHit2D playerInMeleeRange = Physics2D.Raycast(transform.position, playerCheckDirection, attackDistance, playerLayer);


        if (playerInMeleeRange.collider)
        {

            ChangeAnimationState("Slime_Attack");
            isAttacking = true;

        }
        else
        {

            if (movingRight)
            {
                transform.position += new Vector3(+walkSpeed, 0, 0);
                movingRotation = new Vector3(0, 0, 0);
                isOnPlatformEdge = Physics2D.Raycast(transform.position + new Vector3(0.1f, 0, 0), Vector2.down, 1f, layersThatCanBeWalkedOn);
                blockInFront = Physics2D.Raycast(transform.position + new Vector3(0.1f, 0, 0), playerCheckDirection, 0.2f, layersThatCantBeWalkedThrough);
                ChangeAnimationState("Slime_Move");
                playerCheckDirection = Vector2.right;

            }
            else
            {
                transform.position += new Vector3(-walkSpeed, 0, 0);
                movingRotation = new Vector3(0, -180, 0);
                isOnPlatformEdge = Physics2D.Raycast(transform.position + new Vector3(-0.1f, 0, 0), Vector2.down, 1f, layersThatCanBeWalkedOn);
                blockInFront = Physics2D.Raycast(transform.position + new Vector3(0.1f, 0, 0), playerCheckDirection, 0.2f, layersThatCantBeWalkedThrough);

                ChangeAnimationState("Slime_Move");
                playerCheckDirection = Vector2.left;
            }

            if (isOnPlatformEdge.collider == false || blockInFront.collider == true)
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