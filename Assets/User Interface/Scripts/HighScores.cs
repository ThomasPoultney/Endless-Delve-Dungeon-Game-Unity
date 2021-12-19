using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// a class responsible for downloading the leader board values from our database.
/// </summary>
public class HighScores : MonoBehaviour
{
    const string privateCode = "6QWf-bA0k0uZaXVjf9KsLQkwHmTnQRB06IuM6dZ7OBUw";  //Key to Upload New Info
    const string publicCode = "61ad4a3b8f418f1278ec4c49";   //Key to download
    const string webURL = "http://dreamlo.com/lb/"; //  Website the keys are for

    public PlayerScore[] scoreList;
    DisplayHighscores myDisplay;

    public static HighScores instance; //Required for STATIC usability
    void Awake()
    {
        // UploadScore("ABC",90);
        // UploadScore("DEF",80);
        // UploadScore("GHI",70);
        // UploadScore("JKL",60);
        // UploadScore("MNO",50);
        // UploadScore("PQR",40);
        // UploadScore("STU",30);
        // UploadScore("VWX",20);
        // UploadScore("YZA",10);


        instance = this; //Sets Static Instance
        myDisplay = GetComponent<DisplayHighscores>();
    }


    public static void UploadScore(string username, int score)  //CALLED when Uploading new Score to WEBSITE
    {//STATIC to call from other scripts easily
        Debug.Log(instance);
        instance.StartCoroutine(instance.DatabaseUpload(username,score)); //Calls Instance
    }

    IEnumerator DatabaseUpload(string userame, int score) //Called when sending new score to Website
    {
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(userame) + "/" + score);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            print("Upload Successful");
            DownloadScores();
        }
        else print("Error uploading" + www.error);
    }

    public void DownloadScores()
    {
        StartCoroutine("DatabaseDownload");
    }
    IEnumerator DatabaseDownload()
    {
        //WWW www = new WWW(webURL + publicCode + "/pipe/"); //Gets the whole list
        WWW www = new WWW(webURL + publicCode + "/pipe/0/10"); //Gets top 10
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            OrganizeInfo(www.text);
            if(myDisplay != null)
            {
                myDisplay.SetScoresToMenu(scoreList);

            }
        }
        else print("Error uploading" + www.error);
    }

    void OrganizeInfo(string rawData) //Divides Scoreboard info by new lines
    {
        string[] entries = rawData.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
        scoreList = new PlayerScore[entries.Length];
        for (int i = 0; i < entries.Length; i ++) //For each entry in the string array
        {
            string[] entryInfo = entries[i].Split(new char[] {'|'});
            string username = entryInfo[0];
            int score = int.Parse(entryInfo[1]);
            if(!username.Contains("ERROR"))
            {
                scoreList[i] = new PlayerScore(username, score);
            } else
            {
                scoreList[i] = new PlayerScore("Fetching", 0);
            }


            print(scoreList[i].username + ": " + scoreList[i].score);
        }
    }
}

/// <summary>
/// A struct to store the leaderboard variables, it includes the name and score of each player
/// </summary>
public struct PlayerScore //Creates place to store the variables for the name and score of each player
{
    public string username;
    public int score;

    public PlayerScore(string _username, int _score)
    {
        username = _username;
        score = _score;
    }
}