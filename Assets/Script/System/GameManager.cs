using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool is50Mode = false;
    public static GameManager instance;

    private void Awake()
    {
        ManeSingleton();
        Application.targetFrameRate = 60;
    }
    void Start()
    {
        InitVar();
    }


    void InitVar()
    {
        if (!PlayerPrefs.HasKey("Inited"))
        {
            GameRef.SetMusicState(0);

            PlayerPrefs.SetInt("Inited", 1);
        }
    }
    void OnLevelWasLoaded(int level)
    {
        if (level == SceneIndex.ARENATEAM && is50Mode)
        {
            LevelTeamManager.instance.level = 20;
        }
    }

    void ManeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }
}
