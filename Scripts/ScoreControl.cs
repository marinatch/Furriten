using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class ScoreControl : MonoBehaviour
{
    public GameObject panelScore;
    public InputField username;
    public Text scoreText, timeText;

    private int score, time;

    /*public GameObject scoreMenu;
    public InputField username;
    public Text scoreText;*/

    private string setScorePath = "http://localhost/setscore.php";
    public string tableName;

    void Start()
    {
        panelScore.SetActive(false);
       // scoreMenu.SetActive(false);
    }

    public void SetScore(int _score, int _time)
    {
        panelScore.SetActive(true);
        score = _score; 
        time = _time;
        scoreText.text = "Score:" + _score;
        timeText.text = "Time:" + _time;
       
    }

    /*public void SetScore(int _score, int _world)
    {
        if (PlayerPrefs.GetInt("worldScore" + _world) < _score)
        {
            PlayerPrefs.SetInt("worldScore" + _world, _score);
            print("Superas la maxima puntuación con: " + _score);
        }
        panelScore.SetActive(true);
        scoreText.text = _score.ToString();
        
    }*/

    /*public void CancelPanel()
    {
        SceneManager.LoadScene(0);
    }*/
    public void Cancel()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void SaveScore()
    {
        StartCoroutine(sendInfo());
        panelScore.SetActive(false);
       
        Time.timeScale = 1;
    }
    
    IEnumerator sendInfo()
    {
        WWWForm form = new WWWForm();
        form.AddField("tablename", tableName);
        form.AddField("username", username.text);
        form.AddField("score", score);
        form.AddField("time", time);

        UnityWebRequest request = UnityWebRequest.Post(setScorePath, form);
        yield return request.SendWebRequest();
        if(request.isNetworkError||request.isHttpError)
        {
            print(request.downloadHandler.text);
        }
        Cancel();
    }


}