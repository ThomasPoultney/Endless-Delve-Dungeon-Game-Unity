using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected float damage;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") print("HEY! DONT FORGET ABOUT ME! AN ARROW HIT THE PLAYER, BUT HEALTH HASN'T BEEN REDUCED!");
        gameObject.SetActive(false);
        // Reduces player health, but this is not implemented yet. 
        //if (collision.tag == "Player") collision.GetComponent<Health>().TakeDamage(damage);
    }
}