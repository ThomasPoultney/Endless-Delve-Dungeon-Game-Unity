using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    public string currentAnimationState;
    public Animator animator;

    [SerializeField] private float speedConstant;
    private Rigidbody2D playerBody;
    private Animator anim;
    private bool playerGrounded = true;

    //limits how many ropes players can spawn
    [SerializeField] private int numberOfRopesPlayerCanSpawn = 3;
    private List<GameObject> ropesSpawned = new List<GameObject>();
    [SerializeField] private GameObject ropeSpawner;
    private float lastSpawnTime;
    private bool canSpawnRope;
    [SerializeField] private float timeBetweenRopeSpawns = 5;


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
        float horizontalInput = Input.GetAxis("Horizontal");
        playerBody.velocity = new Vector2(speedConstant * horizontalInput, playerBody.velocity.y);


        // Flips player when moving left and right to direction of movement.
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(playerSizeConstant, playerSizeConstant, 1);
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-playerSizeConstant, playerSizeConstant, 1);

        }
        if (Input.GetKey(KeyCode.Space) && playerGrounded)
        {
            Jump();
        }

        //if you are not already attached to a rope you can fire a new rope

        if (!canSpawnRope && Time.time - lastSpawnTime > timeBetweenRopeSpawns)
        {
            canSpawnRope = true;
        }



        if (Input.GetKey(KeyCode.R) && !transform.GetComponent<PlayerRopeTest>().attached && canSpawnRope)
        {
            Debug.Log("Firing Rope");
            fireRope();
        }

        // Set animator parameters
        ChangeAnimationState("Player_Run");
    }

    /// <summary>
    /// Spawns a rope into the world, removes existing ropes if they exceed the limit.
    /// </summary>
    private void fireRope()
    {

        lastSpawnTime = Time.time;
        canSpawnRope = false;
        ChangeAnimationState("Player_Item");

        int numLinksToSpawn = 5;
        if (ropesSpawned.Count >= numberOfRopesPlayerCanSpawn && ropesSpawned.Count > 0)
        {
            GameObject ropeToDelete = ropesSpawned[0];
            ropesSpawned.RemoveAt(0);
            GameObject.Destroy(ropeToDelete); 
        }

        ropeSpawner.GetComponent<spawnRope>().numLinks = numLinksToSpawn;
        GameObject spawnedRope = Instantiate(ropeSpawner, transform.position + new Vector3(0,numLinksToSpawn,0), Quaternion.identity);
        ropesSpawned.Add(spawnedRope);
        
        StartCoroutine(timeDelay());
                 
    }

    IEnumerator timeDelay()
    {
        Debug.Log("Time Delay");
        yield return new WaitForSeconds(3);
    }


    private void Jump()
    {
        playerBody.velocity = new Vector2(playerBody.velocity.x, playerBody.velocity.y + speedConstant);
        ChangeAnimationState("Player_Jump");
        playerGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9) playerGrounded = true;
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
