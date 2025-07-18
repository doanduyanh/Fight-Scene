using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GamePlayController : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject pauseButton;
    [SerializeField]
    private Button musicBtn;
    [SerializeField]
    private Sprite[] musicSprite;
    public MusicManager mM;

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneIndex.MAINMENU);
    }
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
    private void Update()
    {
        CheckMusic();
    }

    void CheckMusic()
    {
        if (GameRef.GetMusicState() == 1)
        {
            musicBtn.image.sprite = musicSprite[1];
        }
        if (GameRef.GetMusicState() == 0)
        {
            musicBtn.image.sprite = musicSprite[0];
        }
    }
    public void Music()
    {
        if (GameRef.GetMusicState() == 1)
        {
            GameRef.SetMusicState(0);
            mM.UpdateMusicState();
            musicBtn.image.sprite = musicSprite[0];
        }
        else if (GameRef.GetMusicState() == 0)
        {

            GameRef.SetMusicState(1);
            mM.UpdateMusicState();
            musicBtn.image.sprite = musicSprite[1];
        }
    }
}
