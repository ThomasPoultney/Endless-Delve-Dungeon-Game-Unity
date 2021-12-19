using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A static Class that can be used to communicate map variables with other objects and classes.
/// </summary>
public static class MapVariables
{
  
    ///All layers that a object can walk on
    public static LayerMask whatIsGround;
    ///All layers that are walls.
    public static LayerMask whatIsWall;
    ///All layers that are interactable.
    public static LayerMask whatIsInteractable;

    ///The current depth the player is at.
    public static int delveLevel = 1;

    ///The current difficult of the world, used to calculate how many mobs will be spawned and insanity drain rate.
    public static float currDifficulty = 1;


    ///how much the difficulty is increased after each level. 
    private static float difficultyIncrease = 0.1f;

    /// <summary>
    /// Returns how many dungeons the player has explored
    /// </summary>
    /// <returns>The current depth of the player</returns>
    public static int GetDelveLevel()
    {
        return delveLevel; 
    }


    /// <summary>
    /// Increments current depth level
    /// </summary>
    public static void IncrementDelveLevel()
    {
        delveLevel++;
    }

    /// <summary>
    /// Sets the depth of the player to the given amount.
    /// </summary>
    /// <param name="amount"></param>
    /// The new depth of the player.
    public static void SetDelveLevel(int amount)
    {
       delveLevel = amount;
    }


    /// <summary>
    /// Updates the current difficulty level to the given amount.
    /// </summary>
    /// <param name="amount"></param>
    /// The new difficulty level.
    public static void SetCurrentDifficulty(float amount)
    {
        currDifficulty = amount;
    }

    /// <summary>
    /// A function that calculated the difficulty of the current dungeon depth.
    /// </summary>
    /// <returns></returns>
    /// The difficulty of the level.
    public static float calcDifficulty()
    {       
        return currDifficulty + (delveLevel * difficultyIncrease);
    }
    static void Start()
    {
        
    }

    // Update is called once per frame
    static void Update()
    {
        
    }
}
