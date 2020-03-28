using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Scene currentScene;

    public int PlayerScore = 0;

    public bool PlayerIsDead = false;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("Error: Duplicated " + this + "in the scene");
        }
    }
    void Update()
    {
        currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "TitleScene")
        {
            if (Input.GetKey(KeyCode.Space)) SceneManager.LoadScene(2);
        }
        else if (currentScene.name == "GameOverScene")
        {
            if (Input.GetKey(KeyCode.Space)) SceneManager.LoadScene(2);
            if (Input.GetKey(KeyCode.Escape)) SceneManager.LoadScene(1);
        }
        else if (currentScene.name == "VictoryScene")
        {
            if (Input.GetKey(KeyCode.Space)) SceneManager.LoadScene(1);
        }
    }

    public void CheckPlayer()
    {
        if (PlayerIsDead == true)
        {
            SceneManager.LoadScene(4);
        }
    }

    public void addPoints(int points)
    {
        PlayerScore += points;
    }
}
