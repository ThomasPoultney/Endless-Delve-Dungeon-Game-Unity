using UnityEngine;

public class SkelentonMobController : MonoBehaviour
{
    public float walkSpeed = 0.2f;

    public Animator animator;
    private bool isAttacking = false;

    private Vector3 rayPosition;
    private bool movingRight = true;
    private string currentAnimationState = "Skelenton_Idle";
    public LayerMask playerLayer;
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
        Vector3 movingRotation;


        RaycastHit2D playerInMeleeRange = Physics2D.Raycast(transform.position, playerCheckDirection, 0.5f, playerLayer);


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