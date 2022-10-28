using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


public class MenuManager : MonoBehaviour
{
    public int currentWorld;
    public List<WorldsProperties> worlds;
    public Text WorldName, maxScore;

    public List<GameObject> FxAudios;
    private bool firstTake;

    private string getScorePath = "http://localhost/getscore.php";

    public Transform gridScore;
    public GameObject baseScore;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
       PlayerPrefs.DeleteAll();
#endif
        //guardarme el current world en el Menu (a continuación en STARTGAME)
        currentWorld = PlayerPrefs.GetInt("currentWorld");

        string keySave = "World" + 0;
        PlayerPrefs.SetInt(keySave, 1);
        SetWorld(0);
    }

    public void SetWorld(int _world)
    {
        //ACLARAR QUE ES "FIRST TAKE"
        if (firstTake == true)
        {
            GameObject newSound = Instantiate(FxAudios[0]);
            Destroy(newSound, 3);
        }
        firstTake = true;

        currentWorld = _world;

        //la obtención de la información del mundo, desde el php en el enternet
        //Acceder a una IEnumenator
        StartCoroutine(GetScoreInfo(currentWorld));


        WorldName.text = worlds[currentWorld].name;
        //
       // maxScore.text = "Max Score: " + PlayerPrefs.GetInt("worldScore" + _world);
        //
        for (int i = 0; i < worlds.Count; i++)
        {
            //un sistema de cargada, para guardar los mundos acabados, debe estar dentro del for pero por encima de los if's
            string keyLoad = "World" + i;
            worlds[i].isActive = PlayerPrefs.GetInt(keyLoad) != 0;

            if(i == _world)
            {
                worlds[i].button.color = Color.white;
            }
            else
            {
                if (worlds[i].isActive == true)
                {
                    worlds[i].button.color = Color.yellow;
                }
                else
                {
                    worlds[i].button.color = Color.gray;

                }
                //inactivar los botones de los mundos donde no he llegado
                worlds[i].button.GetComponent<Button>().interactable = worlds[i].isActive;

            }

        }
    }

    // para acceder al net ( un base de datos o un sitio) usamos IEnumerator
    IEnumerator GetScoreInfo(int _world) //es el id del mundo
    {
        WWWForm form = new WWWForm();
        form.AddField("tablename", worlds[_world].tabelName);

        UnityWebRequest request = UnityWebRequest.Post(getScorePath, form);
        yield return request.SendWebRequest();

        //imprimir la info que me va a devolver del net
        SplitText(request.downloadHandler.text);
    }

    private void SplitText(string _text)

    {
        //limpiar world, asi no se añaden los scores antigous
        worlds[currentWorld].score = new List<ScoreProperties>();
        for (int i = gridScore.childCount - 1; i >= 0; i--)
        {
            Destroy(gridScore.GetChild(i).gameObject);
        }

        // /()/ el elemento de recorte en el php
        // dividir la info por linias
        string[] allRows = _text.Split(new string[] { "/()/" }, System.StringSplitOptions.None);

        //dividir la info en columnas
        for (int i = 0; i < allRows.Length - 1; i++)
        {
            string[] allColumns = allRows[i].Split(new string[] { "/_/" }, System.StringSplitOptions.None);

            //crear un nuevo usario
            ScoreProperties newUser = new ScoreProperties();
            newUser.user = allColumns[0];
            newUser.score = int.Parse(allColumns[1]); //el score es un numero int
            newUser.time = int.Parse(allColumns[2]); //el time es un numero int

            //añadir este info al Worlds
            worlds[currentWorld].score.Add(newUser);
            GameObject newScore = Instantiate(baseScore, gridScore);
            newScore.transform.Find("UserName").GetComponent<Text>().text = newUser.user;
            newScore.transform.Find("Score").GetComponent<Text>().text = "Score: " + newUser.score.ToString();
            newScore.transform.Find("Time").GetComponent<Text>().text = "Time: " + newUser.time.ToString();
        }
    }

    //gestionar el buton Start Game, necesitamos la libreria "SceneManagement"
    public void StartGame()
    {
        //guardarme el current world en el Menu (a continuación en START)
        PlayerPrefs.SetInt("currentWorld", currentWorld);

        SceneManager.LoadScene(worlds[currentWorld].idScene);
    }

    /*public void Credits()
    {
        SceneManager.LoadScene(worlds[currentWorld].idScene);
    }*/
    // Update is called once per frame
    void Update()
    {
        
    }
}


[System.Serializable]
public class WorldsProperties
{
    public bool isActive;
    public string name;
    public int idScene;
    public Image button;
    public string tabelName;

    public List<ScoreProperties> score;
}
[System.Serializable]
public class ScoreProperties
{
    public string user;
    public int score;
    public int time;
}
