using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Responsible for handling all player collisions, including interactions with world object such as doors or ladders.
/// </summary>
public class Player_Collisions : MonoBehaviour
{
    ///whether the player can be dazed.
    public bool canBeDazed;
    /// Whether the player can Die.
    public bool canDie;
    ///whether the player can be knockedDown.
    public bool canBeKnockedDown;
    ///whether the player can be damaged.
    public bool canTakeDamage = true;
    ///whether the player is currently dazed any therefore cannot make inputs.
    [HideInInspector]
    public bool isDazed = false;
    [HideInInspector]
    ///whether the player is currently dying any therefore cannot make inputs.
    public bool isDieing = false;
    [HideInInspector]
   ///whether the player is currently knockedback any therefore cannot make inputs.
    public bool isKnockedBack = false;

    ///How long the player has been knockedback, used for disabling player controls.
    private float timeKnockedBack = 0f;
    ///How long since the player was last knocked back, used to stop mobs permanetly keeping player stunned
    private float timeSinceKnockBack = 0f;

    ///How long the player has been dazed, used for disabling player controls.
    private float timeDazed = 0f;
    ///How long since the player was last dazed, used to stop mobs permanetly keeping player stunned
    private float timeSinceDazed = 0f;

    ///how long the player will spend dieing, used to control animation length.
    private float timeDieing = 0f;
    ///The time the player has currently spent dieing
    private float timeSinceDieing = 0f;
    ///whether the player is alive
    public bool isAlive = true;


    ///The overlap radius that we check for objects to interact with.
    [SerializeField] private float interactRadius = 0.4f;

    ///The length of time that the invicibility frame will last
    [SerializeField] private float iFrameTime = 0.3f;
    ///The length of time since the player last took damage
    private float timeSinceLastDamage = 0f;
    ///whether the player is currently invincible
    private bool invicible = false;


    ///The animation to be played when the player is dazed
    [SerializeField] private AnimationClip dazedAnimation = null;
    ///The animation to be played when the player is dead

    [SerializeField] private AnimationClip deadAnimation = null;
    ///The animation to be played when the player is knockedBack

    [SerializeField] private AnimationClip knockedBackAnimation = null;
    ///The audio source to be played when the player takes damage
    [SerializeField] private AudioSource takeDamageSound = null;
    ///The audio source to be played when the player dies

    [SerializeField] private AudioSource deathSound = null;

    ///The particle system to be spawned when the player takes damage
    [SerializeField] private GameObject bloodSplatter = null;

    ///The layers that the player object should look for when checking if they can interact with an object.   
    [SerializeField] private LayerMask interactionLayer;

    ///an empty game object placed at the players head
    [SerializeField] private Transform headPos;
    ///an empty game object placed at the players feet
    [SerializeField] private Transform feetPos;

    ///The health bar that we change when we take damage
    [SerializeField] private HealthBarScript healthBar;
    ///The prefab menu to load when the player died
    [SerializeField] private EnableDeathMenu enableDeathMenu;
    ///The prefab next level menu to load when the reaches the exit
    [SerializeField] private EnableNextLevelMenu enableNextLevelMenu;
    ///The UI element that we display the current treasure amount on.
    [SerializeField] private UpdateText treasureText;
    ///The animation that the door should play when the player interacts with it
    [SerializeField] private AnimationClip DoorOpenAnimation;
    
    /// <summary>
    /// 
    /// </summary>
    public void Start()
    {
        healthBar.SetMaxHealth(Player_Variables.GetHP());
    }


    /// <summary>
    /// A method that is responsible for applying damage to the player.
    /// 
    /// The method also controls whether the player should be knocked back, the direction they are knocked back and the force they are knocked back with.
    /// If the player drops to 0 hp We run the desired death funcitonality.
    /// </summary>
    /// <param name="amount"></param>
    /// The amount the players health should be changed by, Negative numbers remove health, Positives numbers heal the player
    /// <param name="doesKnockBack"></param>
    /// Whether the player should be knocked back
    /// <param name="knockBacKDirection"></param>
    /// The direction the player should be knockback, Given as one of the primitive vector direction e.g Vector2.Right
    /// <param name="knockBackForce"></param>
    /// The force that the player should be knocked back by.
    public void takeDamage(int amount, bool doesKnockBack, Vector2 knockBacKDirection, float knockBackForce)
    {
        if (!canTakeDamage || invicible || !isAlive)
        {
            return;
        }



        Player_Controller Player_Controller = transform.GetComponent<Player_Controller>();
        
        if (transform.GetComponent<PlayerRopeController>().attached == true)
        {
            transform.GetComponent<PlayerRopeController>().Detach(true);
        }

        Player_Controller.isWallGrabbing = false;
        Player_Controller.isWallSliding = false;
        Player_Controller.canWallGrab = false;
        Player_Controller.wallGrabResetTimer = 0.4f;
        Player_Controller.timeSinceCannotWallGrab = Time.time;

        
        Player_Variables.SetHP(Player_Variables.GetHP() + amount);
        int health = Player_Variables.GetHP();
        invicible = true;
        timeSinceLastDamage = Time.time;
        AnimationController animationController = transform.GetComponent<AnimationController>();


        if (health <= 0 && canDie)
        {

            setPlayerDead();
            
        }
        else if (health > 0 && canBeDazed && !doesKnockBack)
        {
            isDazed = true;
            Rigidbody2D playerBody = transform.GetComponent<Rigidbody2D>();
            playerBody.velocity = Vector2.zero;
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
            
            animationController.ChangeAnimationState(knockedBackAnimation.name);
            timeKnockedBack = animationController.getCurrentAnimationLength();
            timeSinceKnockBack = Time.time;
        }

        healthBar.SetHealth(Player_Variables.GetHP());
        Instantiate(bloodSplatter,transform);
        
    }


    /// <summary>
    /// Sets the player to be dead and enables the death menu to be displayed.
    /// </summary>
    private void setPlayerDead()
    {
        transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        isDieing = true;
        isAlive = false;
        AnimationController animationController = transform.GetComponent<AnimationController>();
        animationController.ChangeAnimationState(deadAnimation.name);
        timeDieing = animationController.getCurrentAnimationLength();
        Debug.Log("Dead");
        timeSinceDieing = Time.time;
        if (deathSound != null)
        {

        }


        healthBar.SetHealth(0);

        enableDeathMenu.Setup(Player_Variables.getTreasure());
    }

    /// <summary>
    /// Monobehaviour which is called every frame, every frame we check for collisions and whether 
    /// the player current condition should be reset e.g. have they been dazed/knocked down long enough
    /// </summary>
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

        if(Time.time - timeSinceLastDamage > iFrameTime)
        {
            invicible = false;
        }


        Vector2 overlapPointOne = new Vector2(headPos.position.x + interactRadius, headPos.position.y);
        Vector2 overlapPointTwo = new Vector2(feetPos.position.x - interactRadius, feetPos.position.y);
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(feetPos.position, interactRadius,interactionLayer);

        if(objectsInRange.Length > 0) 
        {
            foreach(Collider2D obj in objectsInRange)
            {
                if (obj.gameObject.layer == 11) //loot
                {
                    Player_Variables.addTreasure(obj.GetComponent<TreasureValue>().getTreasureValue());
                    treasureText.updateText(Player_Variables.getTreasure().ToString());
                    Destroy(obj.gameObject);
                }

                if (obj.gameObject.layer == 12) //ladder
                {
                    bool touchingLadder = true;
                    Debug.Log("Touching Ladder");
                }

                if (obj.gameObject.layer == 7) //spikes
                {

                    takeDamage(-1, false, Vector2.zero, 0);
                }

                if (obj.gameObject.layer == 13) //lava
                {
                    setPlayerDead();                   
                }

                if(obj.gameObject.layer == 17 && Input.GetKey(KeyCode.E) && obj.gameObject.name == "Exit Door")//Door
                {
                   
                    obj.gameObject.GetComponent<AnimationController>().ChangeAnimationState(DoorOpenAnimation.name);
                    transform.GetComponent<Player_Collisions>().enabled = false;
                    transform.GetComponent<Player_Controller>().enabled = false;
                    enableNextLevelMenu.Setup();

                }


            }
        }

    }

    /// <summary>
    /// Draws debug information in the editor
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(feetPos.position, interactRadius);
    }

}
