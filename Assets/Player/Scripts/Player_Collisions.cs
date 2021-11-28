using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Collisions : MonoBehaviour
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
        Animator playerAnimator = transform.GetComponent<Animator>();

        if (health <= 0 && canDie)
        {
            isDieing = true;
            playerAnimator.Play(deadAnimation.name);


        }
        else if (health > 0 && canBeDazed)
        {
            isDazed = true;

            playerAnimator.Play(dazedAnimation.name);
            timeDazed = playerAnimator.GetCurrentAnimatorStateInfo(0).length;
            timeSinceDazed = Time.time;
            if (takeDamageSound != null)
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
