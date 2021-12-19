using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A script which can be used to set the next level hud menu active when the player interacts with the exit.
/// </summary>
public class EnableNextLevelMenu : MonoBehaviour
{

    /// The number of tourhes to be added to the player when they load a new level.
   
    [SerializeField] private int tourchesAddedOnNewLevel = 3;
    ///The player to be spawned in the next level.
    [SerializeField] private GameObject player;
    ///The hud element to display the amount of treasure collected to.
    public Text treasureText;
    ///The hud element to display the current depth of the player.
    public Text currentDepth;

    ///The world spawning object responsible for spawning the next level.
    [SerializeField] private GameObject worldSpawner;


    /// <summary>
    /// Sets the spawn next level hud active so that it is drawn infront of the camera.
    /// Retrieves the players current score and updates the player variables script.
    /// Increments the difficulty of the level.
    /// </summary>
    /// <param name="score"></param>
    public void Setup()
    {
        treasureText.text = "Treasure Collected: " + Player_Variables.getTreasure().ToString();
        currentDepth.text = "Current Depth: " + MapVariables.GetDelveLevel().ToString();
        GameObject.Destroy(GameObject.Find("World"));
        gameObject.SetActive(true);
       
    }

    /// <summary>
    /// Spawns the next level when the Delve deeper button is clicked.
    /// Updates map and player variables then spawns a new world with these values.
    /// </summary>
    public void SpawnNextLevelButton()
    {
        //resets player Values
        Player_Variables.resetValues();
        player.GetComponentInChildren<InsanityDemonstration>().resetInsanityLight();
        Player_Variables.SetNumberTourches(Player_Variables.GetNumTorches() + tourchesAddedOnNewLevel);      
        MapVariables.IncrementDelveLevel();
        MapVariables.calcDifficulty();
        player.GetComponent<Player_Collisions>().enabled = true;
        player.GetComponent<Player_Controller>().enabled = true;      
        worldSpawner.GetComponent<WorldSpawner>().SpawnWorld();
        gameObject.SetActive(false);
        Debug.Log("Spawning new Level");
    }
}
