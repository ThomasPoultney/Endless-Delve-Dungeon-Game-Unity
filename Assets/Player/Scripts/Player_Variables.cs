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

    private static List<GameObject> itemsInInventory;
    private static int currentNumTorches;



    public static void changeHP(int amount)
    {
        currentHealth += amount;
    }


}
