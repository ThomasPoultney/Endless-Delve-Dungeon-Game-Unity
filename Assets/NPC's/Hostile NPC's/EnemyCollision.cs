using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Handles mob collisions with other objects, currently only collisions with the player.
/// </summary>
public class EnemyCollision : MonoBehaviour
{
    ///The health of the mob
    public int health = 2;
    ///Whether the mob can be dazed
    public bool canBeDazed;
    ///Whether the mob can die
    public bool canDie;
    ///Whether the mob can take damage
    public bool canTakeDamage = true;
    ///Whether the mob is dazed

    [HideInInspector]
    public bool isDazed = false;
    ///Whether the mob is currently dieing

    [HideInInspector]
    public bool isDieing = false;

    ///The time the mob has spent dazed

    private float timeDazed = 0f;
    ///The time since the mob was last dazed.

    private float timeSinceDazed = 0f;

    ///The time the mob has spent dieing.

    private float timeDieing = 0f;
    ///The time since the mob started dieing.
    private float timeSinceDieing = 0f;

    ///The dazed animation the mob should play.
    [SerializeField] private AnimationClip dazedAnimation = null;
    ///The death animation the mob should play.

    [SerializeField] private AnimationClip deadAnimation = null;
    ///The sound that should be played when the mob takes damage.
    [SerializeField] private AudioSource takeDamageSound = null;
    ///The sound that should be played when the mob dies.

    [SerializeField] private AudioSource deathSound = null;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount"></param>
    public void takeDamage(int amount)
    {
        if(!canTakeDamage)
        {
            return;
        }


        health += amount;
        AnimationController animationController = transform.GetComponent<AnimationController>();


        if (health <= 0 && canDie)
        {
            isDieing = true;
            animationController.ChangeAnimationState(deadAnimation.name);
            timeDieing = animationController.getCurrentAnimationLength();
            timeSinceDieing = Time.time;
            if (takeDamageSound != null)
            {

            }


        } else if(health > 0 && canBeDazed)
        {
            isDazed = true;

            animationController.ChangeAnimationState(dazedAnimation.name);
            timeDazed = animationController.getCurrentAnimationLength();
            timeSinceDazed = Time.time;
            if(takeDamageSound != null)
            {
                
            }

        } 
    }


    /// <summary>
    /// Monobehaviour that is called every frame. Checks if the mob has finished being dazed or dieing.
    /// </summary>
    public void Update()
    {
        if (isDazed && Time.time - timeSinceDazed > timeDazed)
        {
            Debug.Log("Daze Over");
            isDazed = false;          
        }

        if (isDieing && Time.time - timeSinceDieing > timeDieing)
        {
            Debug.Log("Dieing Over");
            GameObject.Destroy(gameObject);
            isDieing = false;
        }

    }


}


