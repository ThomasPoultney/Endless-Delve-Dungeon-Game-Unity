using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapVariables
{
    // Start is called before the first frame update

    public static LayerMask whatIsGround;
    public static LayerMask whatIsWall;
    public static LayerMask whatIsInteractable;

    public static int delveLevel = 1;

    public static float currDifficulty = 1;

    private static float difficultyIncrease = 0.1f;

    public static int GetDelveLevel()
    {
        return delveLevel; 
    }

    public static void IncrementDelveLevel()
    {
        delveLevel++;
    }

    public static void SetDelveLevel(int amount)
    {
       delveLevel = amount;
    }

    public static void SetCurrentDifficulty(float amount)
    {
        currDifficulty = amount;
    }

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
