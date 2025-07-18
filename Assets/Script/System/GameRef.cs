using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameRef
{
    public static string IsMusicOn = "IsMusicOn";

    public static void SetMusicState(int state)
    {
        PlayerPrefs.SetInt(GameRef.IsMusicOn, state);
    }
    public static int GetMusicState()
    {
        return PlayerPrefs.GetInt(GameRef.IsMusicOn);
    }

}
