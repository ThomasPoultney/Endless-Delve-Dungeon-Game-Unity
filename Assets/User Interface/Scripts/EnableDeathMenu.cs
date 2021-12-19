using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// A script which can be used to set the death hud active when the player has died.
/// </summary>
public class EnableDeathMenu : MonoBehaviour
{
    ///The text that displays the treasure the player had when he died.
    public Text treasureText;
    
    /// <summary>
    /// Sets the death menu hud active so that it is drawn infront of the camera.
    /// Retrieves the players current score and displays it to the HUD.
    /// The players score is also uploaded to the leaderboard.
    /// </summary>
    /// <param name="score"></param>
    public void Setup(int score)
    {
       gameObject.SetActive(true);
       treasureText.text = "Treasure Collected: "  + score.ToString();
          
       Debug.Log("Uploading Score");
       HighScores.UploadScore(Player_Variables.getUsername(), score);     
      
    }

    /// <summary>
    /// Loads the main menu when the main menu button is clicked
    /// </summary>
    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }



    /// <summary>
    /// Loads the leaderboard menu when the leaderboard button clicked
    /// </summary>
    public void LeaderBoardButton()
    {
        SceneManager.LoadScene("Leaderboard");
    }
}
