using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public int health = 2;
    public bool canBeDazed;
    public bool canDie;

    [HideInInspector]
    public bool isDazed = false;
    [HideInInspector]
    public bool isDieing = false;

    private float timeDazed = 0f;
    private float timeSinceDazed = 0f;

    [SerializeField] private AnimationClip dazedAnimation = null;
    [SerializeField] private AnimationClip deadAnimation = null;
    [SerializeField] private AudioSource takeDamageSound = null;
    [SerializeField] private AudioSource deathSound = null;

    public void takeDamage(int amount)
    {
        health += amount;
        AnimationController animationController = transform.GetComponent<AnimationController>();


        if (health <= 0 && canDie)
        {
            isDieing = true;
            animationController.ChangeAnimationState(deadAnimation.name);
           

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


    public void Update()
    {
        if (isDazed && Time.time - timeSinceDazed > timeDazed)
        {
            Debug.Log("Daze Over");
            isDazed = false;          
        }

    }


}


