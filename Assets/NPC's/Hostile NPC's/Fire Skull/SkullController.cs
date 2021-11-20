using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullController : MonoBehaviour
{

    public float speed;
    private GameObject player;
    private bool hunting = false;
    public float pursuitDistance = 4f;
    public float explodeAtDistance = 2f;
    public GameObject explosion;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {       
        if (player != null)
        {
            if (Vector2.Distance(transform.position, player.transform.position) < explodeAtDistance)
            {
                explode();
            }
            else if (Vector2.Distance(transform.position, player.transform.position) < pursuitDistance)
            {
                Hunt();
            }

        } 

        

        
    }

    private void explode()
    {
        
        GameObject Explosion = Instantiate(explosion, transform.position, Quaternion.identity);
        explosion.transform.localScale = new Vector3(0.5f,0.5f,1);
        Destroy(gameObject);
    }

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
