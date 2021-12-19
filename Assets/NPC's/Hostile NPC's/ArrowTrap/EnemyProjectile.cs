using UnityEngine;

/// <summary>
/// Responsible for handling active projectiles, 
/// </summary>
public class EnemyProjectile : EnemyDamage
{
    ///the speed of the projectile
    [SerializeField] private float speed;
    ///the time since the projectile was fired
    [SerializeField] private float resetTime;
    ///The total lifetime of the projectile
    private float lifetime;

    /// <summary>
    /// Enables the projectile and begins its lifetime
    /// </summary>
    public void ActivateProjectile()
    {
        lifetime = 0;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Monobehaviour that is called every frame, Removes an arrow if it has exceeded its lifetime.
    /// </summary>
    private void Update()
    {
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false);
    }

}