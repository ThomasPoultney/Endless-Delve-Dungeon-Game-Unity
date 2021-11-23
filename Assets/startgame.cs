using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startgame : MonoBehaviour
{
   public void playGame(){

       Scene scene = SceneManager.GetActiveScene();
       Destroy(gameObject);
       SceneManager.LoadScene(scene.buildIndex + 1);
      // SceneManager.MoveGameObjectToScene(Canvas, SceneManager.GetActiveScene());
      

   }
}
