using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A static class containing all variables associated with the player.
/// 
/// Used for communicating player stats to other classes.
/// </summary>
public static class Player_Variables
{
      
    ///The Health the player starts at
    public static int startHealth = 10;
    ///The insantity the player starts at
    public static int startInsanity = 5;
    ///The number of torches the player spawns with
    private static int startNumTorches = 4;

    ///The players current health, defaults to start health
    private static int currentHealth = startHealth;
    ///The players current insanity, defaults to start Insanity
    private static float currentInsanity = startInsanity;
    ///Amount of treasure the player has collected
    private static int amountOfTreasure = 0;
    ///The current score of the player
    private static int score = 0;
    ///The high score of all players
    private static int highscore = 0;
   

    ///The username of the player.
    private static string username = "Anonymous";

    ///A list of items in the players inventory.
    private static List<GameObject> itemsInInventory;
    ///The current number of torches the player is holding, Defaults to the start number of torches.
    private static int currentNumTorches = startNumTorches;



    /// <summary>
    /// adds the given amount to the players hp
    /// </summary>
    /// <param name="amount"></param>
    /// The amount to add to the players hp.
    public static void changeHP(int amount)
    {
        currentHealth += amount;
    }

    /// <summary>
    /// sets the player hp to the given value
    /// </summary>
    /// <param name="amount"></param>
    /// The value to set the players hp to.
    public static void SetHP(int amount)
    {
        currentHealth = amount;
    }


    /// <summary>
    /// Sets the players insanity to the given value
    /// </summary>
    /// <param name="amount"></param>
    /// The value to set the players insanity too
    public static void SetInsanity(float amount)
    {
        currentInsanity = amount;
    }

    /// <summary>
    /// retrieves the players current hp
    /// </summary>
    /// <returns>The current health of the player </returns>
    public static int GetHP()
    {
        return currentHealth;
    }

    /// <summary>
    /// retrieves the players starting hp
    /// </summary>
    /// <returns>The players start hp </returns>
    public static int GetStartHP()
    {
        return startHealth;
    }

    /// <summary>
    /// retrieves the players current insanity
    /// </summary>
    /// <returns>The current insanity of the player </returns>
    public static float GetInsanity()
    {
        return currentInsanity;
    }

    /// <summary>
    /// retrieves the players starting insanity
    /// </summary>
    /// <returns>The players start insanity </returns>
    public static int GetStartInstanity()
    {
        return startInsanity;
    }

    /// <summary>
    /// retrieves the number of torches in the players inventory
    /// </summary>
    /// <returns>The current number of torhces in the players inventory </returns>
    public static int GetNumTorches()
    {
        return currentNumTorches;
    }

    /// <summary>
    /// Sets the number of tourches in the players inventory to the given value.
    /// </summary>
    /// <param name="amount"></param>
    /// The amount of torches to be set in the players inventory
    public static void SetNumberTourches(int amount)
    {
        currentNumTorches = amount;
    }

    /// <summary>
    /// retrieves the players current treasure collected
    /// </summary>
    /// <returns>The amount of treasure the player has collected </returns>
    public static int getTreasure()
    {
        return amountOfTreasure;
    }

    /// <summary>
    /// adds treasure to the players inventory
    /// </summary>
    /// <param name="treasureValue"></param>
    /// The amount of treasure to be added
    public static void addTreasure(int treasureValue)
    {
        amountOfTreasure += treasureValue;
        Debug.Log(amountOfTreasure);
    }

    /// <summary>
    /// Sets the players username
    /// </summary>
    /// <param name="usernameInput"></param>
    /// The name of the user.
    public static void setUsername(string usernameInput)
    {
        username = usernameInput;
    }

    /// <summary>
    /// remoes one tourch from the players inventory
    /// </summary>
    public static void removeTorch()
    {
        currentNumTorches--;
    }

    /// <summary>
    /// Returns the number of torches in the players inventory
    /// </summary>
    /// <returns>The number of torches in the players inventory</returns>
    public static int getNumberOfTorches()
    {
        return currentNumTorches;
    }

    /// <summary>
    /// retrieves the players username
    /// </summary>
    /// <returns>players username</returns>
    public static string getUsername()
    {
          return username;
    }

    /// <summary>
    /// Sets the score of the player to the given value
    /// </summary>
    /// <param name="scoreInput"></param>
    /// The new score of the player
    public static void setScore(int scoreInput)
    {
        score = scoreInput;
    }

    /// <summary>
    /// returns the players current score
    /// </summary>
    /// <returns>The current score</returns>
    public static int getScore()
    {
          return score;
    }

    /// <summary>
    /// sets the players highscore
    /// </summary>
    /// <param name="highScore"></param>
    public static void setHighscore(int highScore)
    {
        highscore = highScore;
    }

    /// <summary>
    /// retrieves the high score of the player
    /// </summary>
    /// <returns>The high score of the player </returns>
    public static int getHighscore()
    {
          return highscore;
    }

    /// <summary>
    /// resets all the players values to their starting amounts
    /// </summary>
    public static void resetValues()
    {
        amountOfTreasure = 0;
        currentHealth = startHealth;
        currentInsanity = startInsanity;
        currentNumTorches = startNumTorches;

    }

}
