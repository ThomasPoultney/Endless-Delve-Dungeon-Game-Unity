using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    
    [SerializeField] protected float damage;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject bla = GameObject.Find("Player");
            Player_Collisions other = (Player_Collisions)bla.GetComponent(typeof(Player_Collisions));
            other.takeDamage(1, true, new Vector2(1, 0), 1);
        }
        gameObject.SetActive(false);
        // Reduces player health, but this is not implemented yet. 
        //if (collision.tag == "Player") collision.GetComponent<Health>().TakeDamage(damage);
    }
}