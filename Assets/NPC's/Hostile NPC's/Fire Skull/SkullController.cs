using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private GameObject player;
    private bool hunting = false;
    [SerializeField]
    private float pursuitDistance = 4f; //distance that we pursue player at
    [SerializeField]
    private float explodeAtDistance = 2f; //distance from player that we explode at
    [SerializeField]
    private GameObject explosion; //The explosion to spawn

    void Start()
    {
        //finds the player object in the scence
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {       
        if (player != null)
        {
            //if we are close enough to explode
            if (Vector2.Distance(transform.position, player.transform.position) < explodeAtDistance)
            {
                explode();
            }
            //if we are close enough to pursue player
            else if (Vector2.Distance(transform.position, player.transform.position) < pursuitDistance)
            {
                Hunt();
                hunting = true;
            } else
            {
                hunting = false;
            }

        } 

        

        
    }
    /// <summary>
    /// Spawns an explosion at the skulls location and then removes the skull
    /// </summary>
    private void explode()
    {
        //spawns explosion at the skulls location
        GameObject Explosion = Instantiate(explosion, transform.position, Quaternion.identity);
        //reduces size of explosion
        explosion.transform.localScale = new Vector3(0.5f,0.5f,1);
        //removes skull
        Destroy(gameObject);
    }

    /// <summary>
    /// Moves the skull towards the player, sets skull to face the players
    /// </summary>
    private void Hunt()
    {
 
        if (transform.position.x <  player.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        } else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
}
