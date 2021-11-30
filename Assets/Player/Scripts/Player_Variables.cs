using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Player_Variables
{
    public static int startHealth = 4;
    public static int startInsanity = 4;
    private static int startNumTorches = 4;

    private static int currentHealth = startHealth;
    private static int currentInsanity = startInsanity;
    private static int amountOfTreasure = 0;
   


    private static string username = "Anonymous";

    private static List<GameObject> itemsInInventory;
    private static int currentNumTorches = startNumTorches;



    public static void changeHP(int amount)
    {
        currentHealth += amount;
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




}
