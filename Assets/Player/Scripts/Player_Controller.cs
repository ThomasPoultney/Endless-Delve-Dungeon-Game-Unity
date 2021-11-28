using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    public string currentAnimationState;
    public Animator animator;

    [SerializeField] private float sprintingSpeedConstant = 5f;
    [SerializeField] private float runningSpeedConstant = 3f;
    [SerializeField] private float crouchingSpeedConstant = 2f;
    [SerializeField] private float walkingSpeedConstant = 1f;
    [SerializeField] private float wallSlideSpeedConstant = 2f;
    [SerializeField] private float jumpingConstant = 2f;
    [SerializeField] private float slideThreshold = 4f;
    [SerializeField] private float walkingThreshold = 2f;
    [SerializeField] private float jumpResetTime = 1f;


    private Rigidbody2D playerBody;
    private Animator anim;

  
    //limits how many ropes players can spawn
    [SerializeField] private int numberOfRopesPlayerCanSpawn = 3;
    private List<GameObject> ropesSpawned = new List<GameObject>();
    [SerializeField] private GameObject ropeSpawner;
    private float lastSpawnTime;
    private float timeSinceLastJump;
    private bool canSpawnRope;
    private bool canJumpAgain;
    public bool facingLeft;

    [SerializeField] private float maxRopeSpawnLength = 5;
    [SerializeField] private float timeBetweenRopeSpawns = 5;
    [SerializeField] private LayerMask ropesCanSpawnOn;

    private bool weaponOut = false;
    private bool climbingLadder = false;
    private bool weaponDrawing = false;
    private bool isJumping = false;
    private bool charecterAttacking = false;
    private bool charecterFiringRope = false;
    private float lastAttackTime = 0;
    private float lastRopeFireTime = 0;
    private float timeSinceWeaponDraw;
    [SerializeField] private float jumpTimeCounter = 0.3f;

    private bool isGrounded;
    private bool isWallSliding;
    private bool isWallGrabbing;
    private bool canWallGrab;

    private bool canClimbLedge;
    private bool isTouchingLedge = false;
    private bool ledgeDetected = false;

    [SerializeField] private Transform attackPos;
    [SerializeField] private LayerMask mobLayerMask;
    [SerializeField] private float attackRange = 0.2f;
    


    public Transform feetPos;
    public Transform ledgeCheck;

    public float checkRadius;
    public LayerMask whatIsGround;


    private bool isWallJumping = false;
    private bool crouching;
    
    //used for checking player Input
    private float horizontalInput;
    private float verticalInput;
    private float jumpInput;
    private bool attackInput;
    private bool crouchingInput;
    private bool weaponDrawInput;
    private bool fireRopeInput;
    private bool walkingInput;
    private bool sprintingInput;
    private bool interactInput;
    //used to reset combos after a given time
    private float timeSinceLastMeleeAttack = 0;
    private float timeSinceLastAirMeleeAttack = 0;
    private float timeSinceLastPunchAttack = 0;
    private float comboResetTime = 1;
    
    //tracks combos
    private int punchMeleeAttackCombo = 0;
    private int meleeAttackCombo = 0;
    private int airMeleeAttackCombo = 0;

    private bool isTouchingWall;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance = 1f;

    AnimationController animationController;

    [SerializeField] private float wallJumpResetTimer = 0.2f;
    private float timeSinceLastWallJump;
    [SerializeField] private Vector2 wallJumpAmount = new Vector2(3,5);


    [SerializeField] private float playerSizeConstant;

    // Start is called before the first frame update
    void Start()
    {
        // Grab references for rigidbody (player object) and animator from respective objects.
        playerBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        animationController = transform.GetComponent<AnimationController>();


    }


    // Update is called once per frame
    void Update()
    {


        crouching = false;
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        attackInput = Input.GetMouseButton(0);
        jumpInput = Input.GetAxis("Jump");
        crouchingInput = Input.GetKey(KeyCode.C);
        sprintingInput = Input.GetKey(KeyCode.LeftShift);
        weaponDrawInput = Input.GetKey(KeyCode.Z);
        fireRopeInput = Input.GetKey(KeyCode.R);
        walkingInput = Input.GetKey(KeyCode.LeftControl);
        interactInput = Input.GetKey(KeyCode.E);

        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

       

        if(Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }


        if (transform.localScale.x < 0)
        {
            facingLeft = true;
        } else
        {
            facingLeft = false;
        }
        
        CheckSurroundings();
        if(!isWallJumping)
        {
            CheckIfWallGrabbing();
        }
       
        CheckLedgeClimb();




    }

 

    private void CheckIfWallGrabbing()
    {
        if(isTouchingWall && !isGrounded && playerBody.velocity.y < 0)
        {
            isWallGrabbing = true;
        } else
        {
            isWallGrabbing = false;
        }
    }

    private void CheckSurroundings()
    {
        Vector2 ledgeCheckPos = new Vector2(ledgeCheck.transform.position.x,ledgeCheck.transform.position.y);
        if (facingLeft)
        {
            isTouchingWall = Physics2D.Raycast(wallCheck.position, -transform.right, wallCheckDistance, whatIsGround);
            isTouchingLedge = Physics2D.Raycast(ledgeCheckPos, -transform.right, wallCheckDistance, whatIsGround);
        }
        else
        {
            isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
            isTouchingLedge = Physics2D.Raycast(ledgeCheckPos, transform.right, wallCheckDistance, whatIsGround);
        }
    }

    private void CheckLedgeClimb()
    {
        if (isTouchingWall && !isTouchingLedge)
        {
            ledgeDetected = true;
        }

    }

    public void FixedUpdate()
    {

        bool isDazed = transform.GetComponent<Player_Collisions>().isDazed;
        bool isDieing = transform.GetComponent<Player_Collisions>().isDieing;
        bool isKnockedBack = transform.GetComponent<Player_Collisions>().isKnockedBack;

        isWallSliding = false;

        if(isDieing || isKnockedBack)
        {
            return;
        }

        if(isDazed)
        {
            setVelocity();
            return;
        }

     

        if (isWallJumping && Time.time - timeSinceLastWallJump > wallJumpResetTimer)
        {
            isWallJumping = false;
            Debug.Log("Can Wall Jump Again");
        }
        else if (isWallJumping)
        {
            if(!facingLeft)
            {
                playerBody.velocity = wallJumpAmount * new Vector2(-1, 1);
            } else
            {
                playerBody.velocity = wallJumpAmount;
            }
            return;
        }

        if (horizontalInput > 0.01)
        {
            transform.localScale = new Vector3(playerSizeConstant, playerSizeConstant, 1);

        }
        else if (horizontalInput < -0.01)
        {
            transform.localScale = new Vector3(-playerSizeConstant, playerSizeConstant, 1);

        }

        if (jumpInput > 0 && isJumping == true && isWallJumping == false)
        {
            if (jumpTimeCounter > 0)
            {
                playerBody.velocity = new Vector2(playerBody.velocity.x, jumpingConstant);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }




        if (charecterAttacking && Time.time - lastAttackTime > animator.GetCurrentAnimatorStateInfo(0).length)
        {
            charecterAttacking = false;
        }
        else if (charecterAttacking)
        {
            return;
        }

        if (charecterFiringRope && Time.time - lastRopeFireTime > animator.GetCurrentAnimatorStateInfo(0).length)
        {
            charecterFiringRope = false;
        }
        else if (charecterFiringRope)
        {
            return;
        }

     
        if (weaponDrawing && Time.time - timeSinceWeaponDraw > animator.GetCurrentAnimatorStateInfo(0).length)
        {
            weaponDrawing = false;
        }
        else if (weaponDrawing)
        {
            return;
        }

    

        if(Time.time - timeSinceLastMeleeAttack >  comboResetTime)
        {
            meleeAttackCombo = 0;
        }

        if (Time.time - timeSinceLastAirMeleeAttack > comboResetTime)
        {
            airMeleeAttackCombo = 0;
        }

        if (Time.time - timeSinceLastPunchAttack > comboResetTime)
        {
            punchMeleeAttackCombo = 0;
        }


        if (!canSpawnRope && Time.time - lastSpawnTime > timeBetweenRopeSpawns)
        {
            canSpawnRope = true;
        }


        bool attached = transform.GetComponent<PlayerRopeTest>().attached;

        //creates a priority list of animations based on certain conditions being met
     
        if (attached)
        {
            animationController.ChangeAnimationState("Player_Wall_Slide");
            // Flips player when moving left and right to direction of movement.
        }
        else if (attackInput && !charecterAttacking) //attacking
        {
            attack();
            if(isGrounded)
            {
                playerBody.velocity = Vector2.zero;
            }
        }
        else if ((isWallGrabbing || isWallSliding) && jumpInput > 0)//and we are losing momentum up we set animation to jump
        {
            Jump();
        }
        else if (isWallGrabbing && verticalInput < 0)//and we are losing momentum up we set animation to jump
        {
            animationController.ChangeAnimationState("Player_Wall_Slide");
            isWallSliding = true;
        }
        else if (isWallGrabbing)//and we are losing momentum up we set animation to jump
        {
            animationController.ChangeAnimationState("Player_Wall_Grab");
        }      
        else if (jumpInput > 0 && isGrounded)
        {
            Jump();
        }
        else if (climbingLadder == true) //climbing Ladder
        {
            animationController.ChangeAnimationState("Player_Climb");
        }
        else if (!isGrounded && playerBody.velocity.y >= 0)//and we are gaining momentum up we set animation to jump
        {
            animationController.ChangeAnimationState("Player_Jump");
        }
        else if (!isGrounded && playerBody.velocity.y <= 0)//and we are losing momentum up we set animation to jump
        {

            animationController.ChangeAnimationState("Player_Fall");
        }
        else if (horizontalInput > 0.01 && crouchingInput)
        {
            animationController.ChangeAnimationState("Player_Crouch_Walk");
            crouching = true;
        }
        else if (horizontalInput < -0.01 && crouchingInput)
        {
            animationController.ChangeAnimationState("Player_Crouch_Walk");
            crouching = true;
        }
        else if (crouchingInput)
        {
            animationController.ChangeAnimationState("Player_Crouch");
            crouching = true;
        }
        else if (verticalInput <= -0.01f && Mathf.Abs(playerBody.velocity.x) > slideThreshold) //SLIDING should check for momentum not input
        {
            animationController.ChangeAnimationState("Player_Ground_Slide");
            crouching = true;
        } else if (horizontalInput > 0.01f && walkingInput)
        {
            animationController.ChangeAnimationState("Player_Walk");
        }
        else if (horizontalInput < -0.01f && walkingInput)
        {
            animationController.ChangeAnimationState("Player_Walk");
        }
        else if (horizontalInput > 0.01f && sprintingInput)
        {
            animationController.ChangeAnimationState("Player_Run_1");
        }
        else if (horizontalInput < -0.01f && sprintingInput)
        {
            animationController.ChangeAnimationState("Player_Run_1");
        }

        else if (horizontalInput > 0.01f)
        {
            animationController.ChangeAnimationState("Player_Run");
        }
        else if (horizontalInput < -0.01f)
        {
            animationController.ChangeAnimationState("Player_Run");
        }
        else if (weaponDrawInput && weaponOut == false)
        {
            animationController.ChangeAnimationState("Player_Sword_Draw");
            weaponOut = true;
            weaponDrawing = true;
            timeSinceWeaponDraw = Time.time;
        }
        else if (weaponDrawInput && weaponOut == true)
        {
            animationController.ChangeAnimationState("Player_Sword_Sheath");
            weaponOut = false;
            weaponDrawing = true;
            timeSinceWeaponDraw = Time.time;
        }
        else if (weaponOut)
        {

            animationController.ChangeAnimationState("Player_Idle");
            currentAnimationState = "Player_Idle";
        }
        else
        {
            animationController.ChangeAnimationState("Player_Idle_Sheathed");
            currentAnimationState = "Player_Idle_Sheathed";
        }



        setVelocity();



        //if you are not already attached to a rope you can fire a new rope
        if (fireRopeInput && !attached && canSpawnRope)
        {
            fireRope();
        }


        

    }

    public void setVelocity()
    {

        
        if (crouching && !charecterAttacking)
        {
            playerBody.velocity = new Vector2(crouchingSpeedConstant * horizontalInput, playerBody.velocity.y);
        }
        else if (walkingInput & !charecterAttacking)
        {
            playerBody.velocity = new Vector2(walkingSpeedConstant * horizontalInput, playerBody.velocity.y);
        }
        else if (sprintingInput & !charecterAttacking)
        {
            playerBody.velocity = new Vector2(sprintingSpeedConstant * horizontalInput, playerBody.velocity.y);
        }
        else if (!charecterAttacking)
        {
            playerBody.velocity = new Vector2(runningSpeedConstant * horizontalInput, playerBody.velocity.y);
        }

        if(isWallSliding)
        {
            playerBody.velocity = new Vector2(0, -wallSlideSpeedConstant);
        }  
        else if(isWallGrabbing)
        {
            
            playerBody.velocity = new Vector2(0, 0);
            
        }
    }

    private void attack()
    {

        attackRange = 0.2f;
        int damageAmount = 1;

        //resets combo
        if(meleeAttackCombo == 3)
        {
            meleeAttackCombo = 0;
        }

        if (punchMeleeAttackCombo == 6)
        {
            punchMeleeAttackCombo = 0;
        }

        if (airMeleeAttackCombo == 3)
        {
            airMeleeAttackCombo = 0;
        }

        if (weaponOut == true)
        {
            if (isGrounded)
            {
                if (meleeAttackCombo == 0)
                {
                    animationController.ChangeAnimationState("Player_Attack_0");
                }
                else if (meleeAttackCombo == 1)
                {
                    animationController.ChangeAnimationState("Player_Attack_1");
                }
                else if (meleeAttackCombo == 2)
                {
                    animationController.ChangeAnimationState("Player_Attack_2");
                }

                meleeAttackCombo++;
                timeSinceLastMeleeAttack = Time.time;
            }
            else if (!isGrounded)
            {
                if (airMeleeAttackCombo == 0)
                {
                    animationController.ChangeAnimationState("Player_Air_Attack_0");
                }
                else if (airMeleeAttackCombo == 1)
                {
                    animationController.ChangeAnimationState("Player_Air_Attack_1");
                }
                else if (airMeleeAttackCombo == 2)
                {
                    animationController.ChangeAnimationState("Player_Air_Attack_2");
                }

                airMeleeAttackCombo++;
                timeSinceLastAirMeleeAttack = Time.time;
            }

            
        }
        else
        {
            if (punchMeleeAttackCombo == 0)
            {
                animationController.ChangeAnimationState("Player_Punch");
            }
            else if (punchMeleeAttackCombo == 1)
            {
                animationController.ChangeAnimationState("Player_Punch_1");
            }
            else if (punchMeleeAttackCombo == 2)
            {
                animationController.ChangeAnimationState("Player_Punch_2");
            }
            else if (punchMeleeAttackCombo == 3)
            {
                animationController.ChangeAnimationState("Player_Punch_3");
            }
            else if (punchMeleeAttackCombo == 4)
            {
                animationController.ChangeAnimationState("Player_Kick");
            }
            else if (punchMeleeAttackCombo == 5)
            {
                animationController.ChangeAnimationState("Player_Kick_1");
            }
          

            
            punchMeleeAttackCombo++;
            timeSinceLastPunchAttack = Time.time;
        }

        Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRange, 1),0,mobLayerMask);
        foreach(Collider2D enemy in enemiesToDamage)
        {
            Debug.Log(enemy);
            enemy.GetComponent<EnemyCollision>().takeDamage(-1);
        }
        
        charecterAttacking = true;
        lastAttackTime = Time.time;
    }


    /// <summary>
    /// Spawns a rope into the world, removes existing ropes if they exceed the limit.
    /// </summary>
    private void fireRope()
    {
        //checks for a solid block above player
        RaycastHit2D ropeSpawnCheck = Physics2D.Raycast(transform.position, Vector2.up, maxRopeSpawnLength,ropesCanSpawnOn);

        int numLinksToSpawn = 0;
        Vector3 ropeSpawnPosition = new Vector3(0,0,0);
        if (ropeSpawnCheck.collider != null)
        {
            Vector3 blockPosition = ropeSpawnCheck.collider.transform.position;
            ropeSpawnPosition = new Vector3(blockPosition.x, blockPosition.y - 1, 0);
            numLinksToSpawn = (int)Math.Floor(ropeSpawnCheck.distance);
        }
        

        lastSpawnTime = Time.time;
        lastRopeFireTime = Time.time;
        canSpawnRope = false;
        charecterFiringRope = true;

        animationController.ChangeAnimationState("Player_Item");

        if (numLinksToSpawn > 0)
        {
            if (ropesSpawned.Count >= numberOfRopesPlayerCanSpawn && ropesSpawned.Count > 0)
            {
                GameObject ropeToDelete = ropesSpawned[0];
                ropesSpawned.RemoveAt(0);
                GameObject.Destroy(ropeToDelete);
            }

            ropeSpawner.GetComponent<spawnRope>().numLinks = numLinksToSpawn;
            GameObject spawnedRope = Instantiate(ropeSpawner, ropeSpawnPosition, Quaternion.identity);
            ropesSpawned.Add(spawnedRope);
        }
    }





    public void Jump()
    {
        if(isWallGrabbing || isWallSliding)
        {
            isWallSliding = false;
            isWallGrabbing = false;
            isWallJumping = true;           
            timeSinceLastWallJump = Time.time;
        }
        else
        {
            playerBody.velocity = Vector2.up * jumpingConstant;
            animationController.ChangeAnimationState("Player_Jump");          
            isJumping = true;
            canJumpAgain = false;
            jumpTimeCounter = jumpResetTime;
            timeSinceLastJump = Time.time;
        }
    }



    void OnDrawGizmos()
    {
       Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
       Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + wallCheckDistance, ledgeCheck.position.y, 0));
       Gizmos.color = Color.blue;
       Gizmos.DrawWireCube(attackPos.position, new Vector2(attackRange,1));
    }


}
