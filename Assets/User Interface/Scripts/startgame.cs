using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startgame : MonoBehaviour
{
   public InputField name; 
   private string input;
   public Button btn;
   public InputField score; 
   private string scoreInput;
   public Button scoreBtn;
   public Button enterAgainBtn;

   private void Start(){
       btn.onClick.AddListener(startGameOnButtonClick);
       scoreBtn.onClick.AddListener(submitScoreOnButtonClick);
       enterAgainBtn.onClick.AddListener(enterScoreAgain);
   } 


    public void startGameOnButtonClick(){
        Debug.Log("Name entered: >>>>> "+name.text);
        Player_Variables.setUsername(name.text);
        Debug.Log("Getting name: >>>>> "+Player_Variables.getUsername());
    }

   public void playGame(string s){

       Time.timeScale = 1;
       if(s!=""){
            Debug.Log("Name entered: >>>>> "+s);
            Player_Variables.setUsername(s);
            Debug.Log("Getting name: >>>>> "+Player_Variables.getUsername());
       }
       Scene scene = SceneManager.GetActiveScene();
       SceneManager.LoadScene(scene.buildIndex + 1);
       Debug.Log("Button Clicked");
       Destroy(gameObject);
    }

    public void enableButton(){
        if(name.text!="")
            btn.interactable = true;
        else
            btn.interactable = false;    
    }

    public void enableScoreButton(){
        if(score.text!="")
            scoreBtn.interactable = true;
        else
            scoreBtn.interactable = false;    
    }

    public void submitScoreOnButtonClick(){
        Debug.Log("Score entered: >>>>> "+score.text);
        Player_Variables.setScore(int.Parse(score.text));
        Debug.Log("Getting score: >>>>> "+Player_Variables.getScore());
        HighScores.UploadScore("TOM",Player_Variables.getScore());
    }

     public void showLeaderboard(int scoreVal){

       Time.timeScale = 1;
       Debug.Log("Score entered 1: >>>>> "+score.text);
       if(score.text!=""){
       Debug.Log("Score entered 2: >>>>> "+score.text);
        Player_Variables.setScore(int.Parse(score.text));
        Debug.Log("Getting score: >>>>> "+Player_Variables.getScore());
        
       }
       Scene scene = SceneManager.GetActiveScene();
       SceneManager.LoadScene(scene.buildIndex + 1);
        HighScores.UploadScore("ABC",90);
        HighScores.UploadScore("DEF",80);
        HighScores.UploadScore("GHI",70);
        HighScores.UploadScore("JKL",60);
        HighScores.UploadScore("MNO",50);
        HighScores.UploadScore("PQR",40);
        HighScores.UploadScore("STU",30);
        HighScores.UploadScore("VWX",20);
        HighScores.UploadScore("YZA",10);

        if(Player_Variables.getScore() > Player_Variables.getHighscore()){
            
                Player_Variables.setHighscore(Player_Variables.getScore());
                HighScores.UploadScore("GARY",Player_Variables.getScore()); 
        }
        Debug.Log("Uploaded score: >>>>> "+Player_Variables.getScore());
       Debug.Log("Submit Score Button Clicked");
       Destroy(gameObject);
    }

    public void enterScoreAgain(){

       Time.timeScale = 1;
       Scene scene = SceneManager.GetActiveScene();
       SceneManager.LoadScene(scene.buildIndex - 1);
       Debug.Log("Enter Again Button Clicked");
       Destroy(gameObject);
    }


    
}

