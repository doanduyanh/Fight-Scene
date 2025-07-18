using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private Button musicBtn;
    [SerializeField]
    private Sprite[] musicSprite;
    public MusicManager mM;
    private void Start()
    {

        CheckMusic();
    }
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

    public void SetMode(bool is50Mode)
    {
        GameManager.instance.is50Mode = is50Mode;
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
