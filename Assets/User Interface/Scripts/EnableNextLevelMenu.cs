using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableNextLevelMenu : MonoBehaviour
{

    [SerializeField] private int tourchesAddedOnNewLevel = 3;
    [SerializeField] private GameObject player;
    public Text treasureText;
    public Text currentDepth;
    [SerializeField] private GameObject worldSpawner;
    // Start is called before the first frame update
    public void Setup()
    {
        treasureText.text = "Treasure Collected: " + Player_Variables.getTreasure().ToString();
        currentDepth.text = "Current Depth: " + MapVariables.GetDelveLevel().ToString();
        GameObject.Destroy(GameObject.Find("World"));
        gameObject.SetActive(true);
       
    }

    public void SpawnNextLevelButton()
    {

        //resets player Values
        Player_Variables.SetHP(Player_Variables.GetStartHP());
        Player_Variables.SetInsanity(Player_Variables.GetStartInstanity());
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
