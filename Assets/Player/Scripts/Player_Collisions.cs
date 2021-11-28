using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Collisions : MonoBehaviour
{
    public int health = 2;
    public bool canBeDazed;
    public bool canDie;
    public bool canBeKnockedDown;
    public bool canTakeDamage = true;
    [HideInInspector]
    public bool isDazed = false;
    [HideInInspector]
    public bool isDieing = false;
    [HideInInspector]
    public bool isKnockedBack = false;

    private float timeKnockedBack = 0f;
    private float timeSinceKnockBack = 0f;

    private float timeDazed = 0f;
    private float timeSinceDazed = 0f;

    private float timeDieing = 0f;
    private float timeSinceDieing = 0f;
    private bool isAlive = true;

    [SerializeField] private AnimationClip dazedAnimation = null;
    [SerializeField] private AnimationClip deadAnimation = null;
    [SerializeField] private AnimationClip knockedBackAnimation = null;
    [SerializeField] private AudioSource takeDamageSound = null;
    [SerializeField] private AudioSource deathSound = null;

    public void takeDamage(int amount, bool doesKnockBack, Vector2 knockBacKDirection, Vector2 knockBackForce)
    {
        if (!canTakeDamage)
        {
            return;
        }


        health += amount;
        AnimationController animationController = transform.GetComponent<AnimationController>();


        if (health <= 0 && canDie && isAlive)
        {
            isDieing = true;
            isAlive = false;
            animationController.ChangeAnimationState(deadAnimation.name);
            timeDieing = animationController.getCurrentAnimationLength();
            timeSinceDieing = Time.time;
            if (takeDamageSound != null)
            {

            }
        }
        else if (health > 0 && canBeDazed && !doesKnockBack)
        {
            isDazed = true;

            animationController.ChangeAnimationState(dazedAnimation.name);
            timeDazed = animationController.getCurrentAnimationLength();
            timeSinceDazed = Time.time;
            if (takeDamageSound != null)
            {

            }
        }
        else if (health > 0 && doesKnockBack && canBeKnockedDown)
        {
            isKnockedBack = true;
            Rigidbody2D playerBody = transform.GetComponent<Rigidbody2D>();
            playerBody.velocity = knockBacKDirection * knockBackForce;
            Debug.Log(knockBacKDirection);
            animationController.ChangeAnimationState(knockedBackAnimation.name);
            timeKnockedBack = animationController.getCurrentAnimationLength();
            timeSinceKnockBack = Time.time;
        }
    }


    public void Update()
    {
        if (isDazed && Time.time - timeSinceDazed > timeDazed)
        {
            isDazed = false;
        }

        if (isDieing && Time.time - timeSinceDieing > timeDieing)
        {
            //GameObject.Destroy(gameObject);
            isDieing = false;
        }

        if (isKnockedBack && Time.time - timeSinceKnockBack > timeKnockedBack)
        {
            isKnockedBack = false;
        }

    }

}
