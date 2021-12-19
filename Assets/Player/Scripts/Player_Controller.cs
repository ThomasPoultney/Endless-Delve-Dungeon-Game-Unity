using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls the movement, animations and combat for the player charecter.
/// 
/// This class handles all the animations for the player gameobject. 
/// </summary>
public class Player_Controller : MonoBehaviour
{
    ///The Current Animation playing
    public string currentAnimationState;
    ///The animator that is attached to the player object
    public Animator animator;

    ///The speed the player travels at whilst sprinting
    [SerializeField] private float sprintingSpeedConstant = 5f;
    ///The speed the player travels at whilst running
    [SerializeField] private float runningSpeedConstant = 3f;
    ///The speed the player travels at whilst crouching
    [SerializeField] private float crouchingSpeedConstant = 2f;
    ///The speed the player travels at whilst walking
    [SerializeField] private float walkingSpeedConstant = 1f;
    ///The speed the player travels at whilst sliding down wall.
    [SerializeField] private float wallSlideSpeedConstant = 2f;
    ///The force the player jumps with.
    [SerializeField] private float jumpingConstant = 2f;
    ///The speed the player travels along the ground whilst sliding
    [SerializeField] private float slideThreshold = 4f;
    ///The minimum time between jumps
    [SerializeField] private float jumpResetTime = 1f;


    ///The rigid body of the player, Used for controlling its physics
    private Rigidbody2D playerBody;


    ///limits how many ropes players can spawn
    [SerializeField] private int numberOfRopesPlayerCanSpawn = 3;
    ///A list of ropes that the player has spawned, Used for deleting previous ropes when player has reached the limit.
    private List<GameObject> ropesSpawned = new List<GameObject>();
    ///The rope that is spawned
    [SerializeField] private GameObject ropeSpawner;
    ///Time since spawning last rope.
    private float lastSpawnTime;
    ///whether the player can spawn a new rope.
    private bool canSpawnRope;
    ///Whether the player is facing left.
    public bool facingLeft;

    ///The maxium length of player spawned ropes.
    [SerializeField] private float maxRopeSpawnLength = 5;
    ///The cooldown time on ropes.
    [SerializeField] private float timeBetweenRopeSpawns = 5;
    ///The types of objects a rope can be spawned on.
    [SerializeField] private LayerMask ropesCanSpawnOn;


    ///Whether the player has their weapon out
    private bool weaponOut = false;
    ///Whether the player is climbing a ladder
    private bool climbingLadder = false;
    ///Whether the player weapon is currently in the process of being drawn
    private bool weaponDrawing = false;
    ///Whether the player is currently jumping
    private bool isJumping = false;
    ///Whether the player is currently attacking
    private bool charecterAttacking = false;
    ///Whether the player is currently spawning a rope
    private bool charecterFiringRope = false;
    ///Time since last attack
    private float lastAttackTime = 0;
    ///Time since last rope spawn
    private float lastRopeFireTime = 0;
    ///Time since the last time the players weapon was drawn
    private float timeSinceWeaponDraw;
    ///The reset time of the players jump.
    [SerializeField] private float jumpTimeCounter = 0.3f;


    ///Whether the player is grounded.
    private bool isGrounded;
    ///Whether the player is wall sliding.
    public bool isWallSliding;
    ///Whether the player is grabbing onto a wall.
    public bool isWallGrabbing;
    ///Whether the player meets the critea to grab onto a wall
    public bool canWallGrab = true;
    ///Reset timer for grabbing a wall
    public float timeSinceCannotWallGrab = 0;
    ///Reset timer for grabbing a wall
    public float wallGrabResetTimer = 0.1f;
    ///Whether the player is touching a ledge
    private bool isTouchingLedge = false;
    ///Whether the player is touching able to perform a ledge climb
    private bool ledgeDetected = false;


    ///The position the player attacks at
    [SerializeField] private Transform attackPos;

    ///The layer of mob objects
    [SerializeField] private LayerMask mobLayerMask;
    ///The attack range of the player, used in collision detection.
    [SerializeField] private float attackRange = 0.2f;


    ///An empty game object placed at the players feet.
    public Transform feetPos;
    ///An empty game object placed at the players head.
    public Transform headPosition;


    public float checkRadius;
    ///All the layers the player can walk on.
    public LayerMask whatIsGround;
    ///All the layers the player can wall grab on.
    public LayerMask whatIsWall;

    ///Whether the player is currently wall jumping
    private bool isWallJumping = false;
    ///Whether the player is currently crouching
    private bool crouching;

    ///stores the current A/D and Left arrow/right arrow input
    private float horizontalInput;
    ///stores the current W/S and up arrow/down arrow input
    private float verticalInput;
    ///stores the current jumpInput from the inputManager
    private float jumpInput;
    ///stores the currente attackInput from the inputManager
    private bool attackInput;
    ///stores the current crouchInput from the inputManager
    private bool crouchingInput;
    ///stores the current weapon Drawn input from the inputManager

    private bool weaponDrawInput;
    ///stores the current fire rope input from the inputManager
    private bool fireRopeInput;
    ///stores the current walking input from the inputManager

    private bool walkingInput;
    ///stores the current sprinting input from the inputManager
    private bool sprintingInput;
    ///stores the current interaction input from the inputManager
    private bool interactInput;
    ///stores the current torch input from the inputManager
    private bool torchInput;

    ///stores the the time since the last ground melee attack, used for calculating Cooldown time on attacks
    private float timeSinceLastMeleeAttack = 0;
    ///stores the the time since the last air melee attack, used for calculating Cooldown time on attacks
    private float timeSinceLastAirMeleeAttack = 0;
    ///stores the the time since the last unarmed melee attack, used for calculating Cooldown time on attacks
    private float timeSinceLastPunchAttack = 0;
    ///The reset time on combos
    private float comboResetTime = 1;

    ///tracks current unarmed melee combo number
    private int punchMeleeAttackCombo = 0;
    ///tracks current armed melee combo number
    private int meleeAttackCombo = 0;
    ///tracks current armed air melee combo number
    private int airMeleeAttackCombo = 0;

    ///whether the player is touching a wall
    private bool isTouchingWall;
    ///Empty game object placed at the position the player should check for a wall.
    [SerializeField] private Transform wallCheck;
    ///The distance from the wallCheck object that we should peform a raycast to check for wall.
    [SerializeField] private float wallCheckDistance = 1f;


    ///The animation controller attached to the player.
    AnimationController animationController;


    ///The length of time before a player can wall jump again
    [SerializeField] private float wallJumpResetTimer = 0.2f;
    ///The length of time since the player last performed a wall jump.
    private float timeSinceLastWallJump;

    ///A vector containing the player performing a wall jump should have
    [SerializeField] private Vector2 wallJumpAmount = new Vector2(3, 5);

    ///The amount we scale player up by.
    [SerializeField] private float playerSizeConstant;


    ///The tourch object that the player can spawn
    [SerializeField] private GameObject torch;
    ///The tourch layer.
    [SerializeField] private LayerMask torchLayer;



    /// <summary>
    /// Monobehaviour that is called before the first frame, In it we get the rigid body and the animation controller of the object the script is attached too.
    /// </summary>
    void Start()
    {
        // Grab references for rigidbody (player object) and animator from respective objects.
        playerBody = GetComponent<Rigidbody2D>();
        animationController = transform.GetComponent<AnimationController>();


    }


    /// <summary>
    /// Monobehaviour that is called every frame, In it we check for all user inputs and check what states the player currently.
    /// </summary>
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
        torchInput = Input.GetKey(KeyCode.F);

        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);



        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }


        if (transform.localScale.x < 0)
        {
            facingLeft = true;
        }
        else
        {
            facingLeft = false;
        }

        CheckSurroundings();
        if (!isWallJumping)
        {
            CheckIfWallGrabbing();
        }

        CheckLedgeClimb();




    }


    /// <summary>
    /// Checks if the player is currently grabbing a wall
    /// </summary>
    private void CheckIfWallGrabbing()
    {
        if (isTouchingWall && !isGrounded && playerBody.velocity.y < 0 && canWallGrab)
        {
            isWallGrabbing = true;
        }
        else
        {
            isWallGrabbing = false;
        }
    }

    /// <summary>
    /// Checks if the playing is touching a wall or is touching a ledge
    /// </summary>
    private void CheckSurroundings()
    {
        Vector2 headPositionPos = new Vector2(headPosition.transform.position.x, headPosition.transform.position.y);
        if (facingLeft)
        {
            isTouchingWall = Physics2D.Raycast(wallCheck.position, -transform.right, wallCheckDistance, whatIsWall);
            isTouchingLedge = Physics2D.Raycast(headPositionPos, -transform.right, wallCheckDistance, whatIsWall);
        }
        else
        {
            isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsWall);
            isTouchingLedge = Physics2D.Raycast(headPositionPos, transform.right, wallCheckDistance, whatIsWall);
        }
    }

    /// <summary>
    /// Checks if the player is able to perform a ledge climb
    /// </summary>
    private void CheckLedgeClimb()
    {
        if (isTouchingWall && !isTouchingLedge)
        {
            ledgeDetected = true;
        }

    }


    /// <summary>
    /// Monobehaviour that is called at a fixed timestamp, In this behaviour we control which animation the player should be executing
    /// based on inputs and player states.
    /// 
    /// </summary>
    public void FixedUpdate()
    {

        bool isDazed = transform.GetComponent<Player_Collisions>().isDazed;
        bool isDieing = transform.GetComponent<Player_Collisions>().isDieing;
        bool isAlive = transform.GetComponent<Player_Collisions>().isAlive;
        bool isKnockedBack = transform.GetComponent<Player_Collisions>().isKnockedBack;

        isWallSliding = false;

        if (isDieing || isKnockedBack || !isAlive)
        {

            return;
        }


        Collider2D[] torches = Physics2D.OverlapCircleAll(transform.position, 2f, torchLayer);
        
        if (torchInput == true && torches.Length == 0 && Player_Variables.getNumberOfTorches() >= 1)
        {
            Instantiate(torch, transform.position, Quaternion.identity);
            Player_Variables.removeTorch();
        }



        if (canWallGrab == false && Time.time - timeSinceCannotWallGrab > wallGrabResetTimer)
        {
            canWallGrab = true;

        }



        if (isWallJumping && Time.time - timeSinceLastWallJump > wallJumpResetTimer)
        {
            isWallJumping = false;

        }
        else if (isWallJumping)
        {
            if (!facingLeft)
            {
                playerBody.velocity = wallJumpAmount * new Vector2(-1, 1);
            }
            else
            {
                playerBody.velocity = wallJumpAmount;
            }
            return;
        }

        if (horizontalInput > 0.01 && !isWallGrabbing)
        {
            transform.localScale = new Vector3(playerSizeConstant, playerSizeConstant, 1);

        }
        else if (horizontalInput < -0.01 && !isWallGrabbing)
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

        if (isDazed)
        {
            setVelocity();
            if (jumpInput > 0 && isGrounded)
            {
                Jump();
            }
            return;
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



        if (Time.time - timeSinceLastMeleeAttack > comboResetTime)
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


        bool attached = transform.GetComponent<PlayerRopeController>().attached;

        //creates a priority list of animations based on certain conditions being met

        if (attached)
        {
            animationController.ChangeAnimationState("Player_Wall_Slide");
            // Flips player when moving left and right to direction of movement.
        }
        else if (attackInput && !charecterAttacking) //attacking
        {
            attack();
            if (isGrounded)
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
        }
        else if (horizontalInput > 0.01f && walkingInput)
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
        else if (horizontalInput > 0.01f && weaponOut == true)
        {
            animationController.ChangeAnimationState("Player_Run_With_Sword");
        }
        else if (horizontalInput < -0.01f && weaponOut == true)
        {
            animationController.ChangeAnimationState("Player_Run_With_Sword");

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

        }
        else
        {
            animationController.ChangeAnimationState("Player_Idle_Sheathed");

        }


        setVelocity();



        //if you are not already attached to a rope you can fire a new rope
        if (fireRopeInput && !attached && canSpawnRope)
        {
            fireRope();
        }

    }

    /// <summary>
    /// updates the velocity of the player based on user inputs and the current state of the player.
    /// </summary>
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




        if (isWallSliding)
        {
            playerBody.velocity = new Vector2(0, -wallSlideSpeedConstant);
        }
        else if (isWallGrabbing)
        {

            playerBody.velocity = new Vector2(0, 0);

        }
    }

    /// <summary>
    /// Sets the player to attack, sets attack animations and tracks attack combos.
    /// </summary>
    private void attack()
    {

        attackRange = 0.2f;
        int damageAmount = 1;

        //resets combo
        if (meleeAttackCombo == 3)
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

        Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRange, 1), 0, mobLayerMask);
        foreach (Collider2D enemy in enemiesToDamage)
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
        RaycastHit2D ropeSpawnCheck = Physics2D.Raycast(transform.position, Vector2.up, maxRopeSpawnLength, ropesCanSpawnOn);

        int numLinksToSpawn = 0;
        Vector3 ropeSpawnPosition = new Vector3(0, 0, 0);
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





    /// <summary>
    /// sets the player to jump or wall jump if the player is on the wall and changes animation state to jumping.
    /// </summary>
    public void Jump()
    {
        if (isWallGrabbing || isWallSliding)
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
            jumpTimeCounter = jumpResetTime;
        }
    }



    /// <summary>
    /// Debugging gizmos for player colliders that are included in the scene view.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
        Gizmos.DrawLine(headPosition.position, new Vector3(headPosition.position.x + wallCheckDistance, headPosition.position.y, 0));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(attackPos.position, new Vector2(attackRange, 1));
        Gizmos.DrawWireSphere(transform.position, 2);
    }


}
