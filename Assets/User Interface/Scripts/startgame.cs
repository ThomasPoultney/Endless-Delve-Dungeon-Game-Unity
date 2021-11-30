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

   private void Start(){
       btn.onClick.AddListener(startGameOnButtonClick);
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

    
}

