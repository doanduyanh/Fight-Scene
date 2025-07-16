using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGameSingle()
    {
        SceneManager.LoadScene(SceneIndex.ARENA);
    }
    public void StartGameMulti()
    {
        SceneManager.LoadScene(SceneIndex.ARENAMULTI);
    }
    public void StartGameTeam()
    {
        SceneManager.LoadScene(SceneIndex.ARENATEAM);
    }
    public void Quit()
    {
        Application.Quit();
    }

}
