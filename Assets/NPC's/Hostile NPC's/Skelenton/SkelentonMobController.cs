using UnityEngine;

public class SkelentonMobController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 0.2f;
    [SerializeField]
    private float attackDistance = 0.5f;
    public Animator animator;
    private bool isAttacking = false;
    private bool facingLeft = false;

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

    private bool dazedTime;
    private bool dazedLength;

    private bool skellyAttacking;
    private float lastAttackTime = 0;

    [SerializeField] private float attackRange = 0.2f;
    [SerializeField] private Transform attackPos;
    [SerializeField] private Vector2 knockBackForce = new Vector2(10, 15);


    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }


    void FixedUpdate()
    {

        if (transform.eulerAngles.y > 0)
        {
            facingLeft = true;
        }
        else
        {
            facingLeft = false;
        }

        bool isDazed = transform.GetComponent<EnemyCollision>().isDazed;
        bool isDieing = transform.GetComponent<EnemyCollision>().isDieing;

        AnimationController animationController = transform.GetComponent<AnimationController>();

        
        if(isDazed || isDieing)
        {
            return;
        }

        if (isAttacking && Time.time - lastAttackTime > animator.GetCurrentAnimatorStateInfo(0).length)
        {
            isAttacking = false;
        }
        else if (isAttacking)
        {
            return;
        }

        RaycastHit2D isOnPlatformEdge;
        RaycastHit2D blockInFront;
        Vector3 movingRotation;

        //ray to check if player is withing attack range
        Collider2D[] playersToDamage = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRange, 1), 0, playerLayer);
     
        if (playersToDamage.Length > 0 && !isAttacking)
        {
            foreach (Collider2D player in playersToDamage)
            {
                Vector2 knockbackDirection;
                if (facingLeft)
                {
                    knockbackDirection = Vector2.left;
                } else
                {
                    knockbackDirection = Vector2.right;
                }

                player.GetComponent<Player_Collisions>().takeDamage(-1, true, knockbackDirection, knockBackForce);
            }

            animationController.ChangeAnimationState("Skelenton_Swing");
            isAttacking = true;
            lastAttackTime = Time.time;

        }
        else
        {

            if (movingRight)
            {
                transform.position += new Vector3(+walkSpeed, 0, 0);
                //sets the direction object is facing to right
                movingRotation = new Vector3(0, -180, 0);
                //checks if we are at platform end
                isOnPlatformEdge = Physics2D.Raycast(transform.position + new Vector3(0.1f, 0, 0), Vector2.down, 1f,layersThatCanBeWalkedOn);
                //checks if we can move forward
                blockInFront = Physics2D.Raycast(transform.position + new Vector3(0.1f, 0, 0), playerCheckDirection, 0.2f, layersThatCantBeWalkedThrough);
                animationController.ChangeAnimationState("Skelenton_Walk");
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

                animationController.ChangeAnimationState("Skelenton_Walk");
                playerCheckDirection = Vector2.left;
            }

            if (isOnPlatformEdge.collider == false || blockInFront.collider == true)
            {
                movingRight = !movingRight;
                transform.eulerAngles = movingRotation;

            }
        }
    }


 
    void OnDrawGizmos()
    {     
        Gizmos.DrawWireCube(attackPos.position, new Vector2(attackRange, 1));
    }
}