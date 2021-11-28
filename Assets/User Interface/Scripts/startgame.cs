using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startgame : MonoBehaviour
{
   public void playGame(){

       Time.timeScale = 1;
       Scene scene = SceneManager.GetActiveScene();
       SceneManager.LoadScene(scene.buildIndex + 1);
       Debug.Log("Button Clicked");
       Destroy(gameObject);



    }
}
