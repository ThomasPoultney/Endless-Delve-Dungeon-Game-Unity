using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Player_Variables
{
    public static int startHealth = 10;
    public static int startInsanity = 4;
    private static int startNumTorches = 4;

    private static int currentHealth = startHealth;
    private static int currentInsanity = startInsanity;
    private static int amountOfTreasure = 0;
    private static int score = 0;
    private static int highscore = 0;
   


    private static string username = "Anonymous";

    private static List<GameObject> itemsInInventory;
    private static int currentNumTorches = startNumTorches;



    public static void changeHP(int amount)
    {
        currentHealth += amount;
    }

    public static void SetHP(int amount)
    {
        currentHealth = amount;
    }

    public static void SetInsanity(int amount)
    {
        currentHealth = amount;
    }


    public static int GetHP()
    {
        return currentHealth;
    }


    public static int GetStartHP()
    {
        return startHealth;
    }


    public static int GetInsanity()
    {
        return currentInsanity;
    }


    public static int GetStartInstanity()
    {
        return startInsanity;
    }

    public static int GetNumTorches()
    {
        return currentNumTorches;
    }

    public static void SetNumberTourches(int amount)
    {
        currentNumTorches = amount;
    }

    public static int getTreasure()
    {
        return amountOfTreasure;
    }

    public static void addTreasure(int treasureValue)
    {
        amountOfTreasure += treasureValue;
        Debug.Log(amountOfTreasure);
    }

    public static void setUsername(string usernameInput)
    {
        username = usernameInput;
    }

    public static void removeTorch()
    {
        currentNumTorches--;
    }

    public static int getNumberOfTorches()
    {
        return currentNumTorches;
    }

    public static string getUsername()
    {
          return username;
    }

    public static void setScore(int scoreInput)
    {
        score = scoreInput;
    }

    public static int getScore()
    {
          return score;
    }

    public static void setHighscore(int highScore)
    {
        highscore = highScore;
    }

    public static int getHighscore()
    {
          return highscore;
    }


    public static void resetValues()
    {
        amountOfTreasure = 0;
        currentHealth = startHealth;
        currentInsanity = startInsanity;
        currentNumTorches = startNumTorches;

    }

}
