using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnableDeathMenu : MonoBehaviour
{

    public Text treasureText;
    // Start is called before the first frame update
    
    public void Setup(int score)
    {
       gameObject.SetActive(true);
       treasureText.text = "Treasure Collected: "  + score.ToString();
          
       Debug.Log("Uploading Score");
       HighScores.UploadScore(Player_Variables.getUsername(), score);     
      
    }
    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }



    // Update is called once per frame
    public void LeaderBoardButton()
    {
        SceneManager.LoadScene("Leaderboard");
    }
}
