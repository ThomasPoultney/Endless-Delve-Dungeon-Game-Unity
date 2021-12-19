using UnityEngine;

/// <summary>
/// Class responsible for controlling the arrow firing traps.
/// 
/// The class spawns an arrow at an given increment
/// </summary>
public class ArrowTrap : MonoBehaviour
{
    ///time between attacks
    [SerializeField] private float attackCooldown;
    ///empty game object placed at the position the trap should fire from
    [SerializeField] private Transform firePoint;
    ///object to be fired
    [SerializeField] private GameObject[] arrows;
    ///cooldown of attacks
    private float cooldownTimer;

    /// <summary>
    /// Spawns an arrow that is fires from traps
    /// </summary>
    private void Attack()
    {
        cooldownTimer = 0;

        arrows[FindArrow()].transform.position = firePoint.position;
        arrows[FindArrow()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    /// <summary>
    /// Chooses the arrow to be spawned
    /// </summary>
    /// <returns></returns>
    private int FindArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
   
    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= attackCooldown)
            Attack();
    }
}