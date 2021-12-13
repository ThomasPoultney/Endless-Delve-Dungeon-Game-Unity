using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    
    [SerializeField] protected float damage;


    /// <summary>
    /// When the arrow enters a 2D collidable object, this method is called.
    /// If the object has tag "Player", then the player has been hit.
    /// Knockback is applied, and one health is taken away.
    /// </summary>
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject playerObject = GameObject.Find("Player");
            Player_Collisions playerScript = (Player_Collisions)playerObject.GetComponent(typeof(Player_Collisions));
            playerScript.takeDamage(-1, true, new Vector2(1, 0), 1);
        }
        gameObject.SetActive(false);
        // Reduces player health, but this is not implemented yet. 
        //if (collision.tag == "Player") collision.GetComponent<Health>().TakeDamage(damage);
    }
}