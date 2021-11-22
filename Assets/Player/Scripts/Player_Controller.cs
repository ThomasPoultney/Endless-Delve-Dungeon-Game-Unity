using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    public string currentAnimationState;
    public Animator animator;

    [SerializeField] private float runningSpeedConstant = 4;
    [SerializeField] private float crouchingSpeedConstant = 2;
    [SerializeField] private float jumpingConstant = 2;
    [SerializeField] private float slideThreshold = 4;
    private Rigidbody2D playerBody;
    private Animator anim;
    private bool playerGrounded = true;
  
    //limits how many ropes players can spawn
    [SerializeField] private int numberOfRopesPlayerCanSpawn = 3;
    private List<GameObject> ropesSpawned = new List<GameObject>();
    [SerializeField] private GameObject ropeSpawner;
    private float lastSpawnTime;
    private bool canSpawnRope;
    [SerializeField] private float maxRopeSpawnLength = 5;
    [SerializeField] private float timeBetweenRopeSpawns = 5;
    [SerializeField] private LayerMask ropesCanSpawnOn;
    private bool weaponOut = false;
    private bool climbingLadder = false;
    private bool weaponDrawing = false;
    private bool charecterAttacking = false;
    private bool charecterFiringRope = false;
    private float lastAttackTime = 0;
    private float lastRopeFireTime = 0;
    private float timeSinceWeaponDraw;
    [SerializeField] private float attackDuration = 1f;

    private bool running;
    private bool walking;
    private bool crouching;

    private float horizontalInput;
    private float verticalInput;
    private float jumpInput;
    private bool attackInput;
    private bool crouchingInput;
    private bool weaponDrawInput;
    private bool fireRopeInput;



    private float timeSinceLastMeleeAttack = 0;
    private float timeSinceLastAirMeleeAttack = 0;
    private float timeSinceLastPunchAttack = 0;
    private float comboResetTime = 1;


    private int punchMeleeAttackCombo = 0;
    private int meleeAttackCombo = 0;
    private int airMeleeAttackCombo = 0;
    


    [SerializeField] private float playerSizeConstant;

    // Start is called before the first frame update
    void Start()
    {
        // Grab references for rigidbody (player object) and animator from respective objects.
        playerBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        attackInput = Input.GetMouseButton(0);
        jumpInput = Input.GetAxis("Jump");
        crouchingInput = Input.GetKey(KeyCode.LeftShift);
        weaponDrawInput = Input.GetKey(KeyCode.Z);
        fireRopeInput = Input.GetKey(KeyCode.R);


    }

    public void FixedUpdate()
    {

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


        if (crouching)
        {
            playerBody.velocity = new Vector2(crouchingSpeedConstant * horizontalInput, playerBody.velocity.y);

        }
        else
        {
            playerBody.velocity = new Vector2(runningSpeedConstant * horizontalInput, playerBody.velocity.y);

        }



        if (!canSpawnRope && Time.time - lastSpawnTime > timeBetweenRopeSpawns)
        {
            canSpawnRope = true;
        }


        bool attached = transform.GetComponent<PlayerRopeTest>().attached;

        //creates a priority list of animations based on certain conditions being met
        if (attached)
        {
            ChangeAnimationState("Player_Wall_Slide");
            currentAnimationState = "Player_Climb";
            // Flips player when moving left and right to direction of movement.
        }
        else if (attackInput && !charecterAttacking) //attacking
        {
            attack();
        }
        else if (jumpInput > 0 && playerGrounded)
        {
            Jump();
        }
        else if (climbingLadder == true) //climbing Ladder
        {
            ChangeAnimationState("Player_Climb");
            currentAnimationState = "Player_Climb";
        }
        else if (!playerGrounded && playerBody.velocity.y >= 0)//and we are gaining momentum up we set animation to jump
        {
            ChangeAnimationState("Player_Jump");
            currentAnimationState = "Player_Jump";
        }
        else if (!playerGrounded && playerBody.velocity.y <= 0)//and we are losing momentum up we set animation to jump
        {

            ChangeAnimationState("Player_Fall");
            currentAnimationState = "Player_Fall";
        }
        else if (horizontalInput > 0.01 && crouchingInput)
        {
            Debug.Log("crouch Walking");
            ChangeAnimationState("Player_Crouch_Walk");
            currentAnimationState = "Player_Crouch_Walk";
            transform.localScale = new Vector3(playerSizeConstant, playerSizeConstant, 1);
            crouching = true;
        }
        else if (horizontalInput < -0.01 && crouchingInput)
        {
            Debug.Log("crouch Walking");
            ChangeAnimationState("Player_Crouch_Walk");
            currentAnimationState = "Player_Crouch_Walk";
            transform.localScale = new Vector3(-playerSizeConstant, playerSizeConstant, 1);
            crouching = true;
        }
        else if (crouchingInput)
        {
            ChangeAnimationState("Player_Crouch");
            currentAnimationState = "Player_Crouch";
            transform.localScale = new Vector3(playerSizeConstant, playerSizeConstant, 1);
            crouching = true;
        }
        else if (verticalInput <= -0.01f && playerBody.velocity.x > slideThreshold) //SLIDING should check for momentum not input
        {
            ChangeAnimationState("Player_Ground_Slide");
            currentAnimationState = "Player_Ground_Slide";
            transform.localScale = new Vector3(playerSizeConstant, playerSizeConstant, 1);
            crouching = true;
        }
        else if (verticalInput <= -0.01f && playerBody.velocity.x < -slideThreshold) //SLIDING should check for momentum not input
        {
            ChangeAnimationState("Player_Ground_Slide");
            currentAnimationState = "Player_Ground_Slide";
            transform.localScale = new Vector3(-playerSizeConstant, playerSizeConstant, 1);
            crouching = true;
        }
        else if (horizontalInput > 0.01f)
        {
            ChangeAnimationState("Player_Run");
            currentAnimationState = "Player_Run";
            transform.localScale = new Vector3(playerSizeConstant, playerSizeConstant, 1);
        }
        else if (horizontalInput < -0.01f)
        {
            ChangeAnimationState("Player_Run");
            currentAnimationState = "Player_Run";
            transform.localScale = new Vector3(-playerSizeConstant, playerSizeConstant, 1);
        }
        else if (weaponDrawInput && weaponOut == false)
        {
            ChangeAnimationState("Player_Sword_Draw");
            currentAnimationState = "Player_Sword_Draw";
            weaponOut = true;
            weaponDrawing = true;
            timeSinceWeaponDraw = Time.time;
        }
        else if (weaponDrawInput && weaponOut == true)
        {
            ChangeAnimationState("Player_Sword_Sheath");
            currentAnimationState = "Player_Sword_Sheath";
            weaponOut = false;
            weaponDrawing = true;
            timeSinceWeaponDraw = Time.time;
        }
        else if (weaponOut)
        {

            ChangeAnimationState("Player_Idle");
            currentAnimationState = "Player_Idle";
        }
        else
        {
            ChangeAnimationState("Player_Idle_Sheathed");
            currentAnimationState = "Player_Idle_Sheathed";
        }

        //if you are not already attached to a rope you can fire a new rope
        if (fireRopeInput && !attached && canSpawnRope)
        {
            fireRope();
        }
    }

    private void attack()
    {
       
        
        //resets combo
        if(meleeAttackCombo == 3)
        {
            meleeAttackCombo = 0;
        }

        if (punchMeleeAttackCombo == 4)
        {
            punchMeleeAttackCombo = 0;
        }

        if (airMeleeAttackCombo == 3)
        {
            airMeleeAttackCombo = 0;
        }

        if (weaponOut == true)
        {
            if (playerGrounded)
            {
                if (meleeAttackCombo == 0)
                {
                    ChangeAnimationState("Player_Attack_0");
                }
                else if (meleeAttackCombo == 1)
                {
                    ChangeAnimationState("Player_Attack_1");
                }
                else if (meleeAttackCombo == 2)
                {
                    ChangeAnimationState("Player_Attack_2");
                }

                meleeAttackCombo++;
                timeSinceLastMeleeAttack = Time.time;
            }
            else if (!playerGrounded)
            {
                if (airMeleeAttackCombo == 0)
                {
                    ChangeAnimationState("Player_Air_Attack_0");
                }
                else if (airMeleeAttackCombo == 1)
                {
                    ChangeAnimationState("Player_Air_Attack_1");
                }
                else if (airMeleeAttackCombo == 2)
                {
                    ChangeAnimationState("Player_Air_Attack_2");
                }

                airMeleeAttackCombo++;
                timeSinceLastAirMeleeAttack = Time.time;
            }

            
        }
        else
        {
            if (punchMeleeAttackCombo == 0)
            {
                ChangeAnimationState("Player_Punch");
            }
            else if (punchMeleeAttackCombo == 1)
            {
                ChangeAnimationState("Player_Punch_1");
            }
            else if (punchMeleeAttackCombo == 2)
            {
                ChangeAnimationState("Player_Punch_2");
            }else if (punchMeleeAttackCombo == 3)
            {
                ChangeAnimationState("Player_Punch_3");
            }
            punchMeleeAttackCombo++;
            timeSinceLastPunchAttack = Time.time;
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

        ChangeAnimationState("Player_Item");

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
        playerBody.velocity = new Vector2(playerBody.velocity.x, playerBody.velocity.y + jumpingConstant);
        ChangeAnimationState("Player_Jump");
        currentAnimationState = "Player_Jump";
        playerGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        climbingLadder = false;
        playerGrounded = false;
        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 16) playerGrounded = true; //9 - basic bloc, 16 - Bedrock

        if (collision.gameObject.layer == 12) climbingLadder = true; //ladder 
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
