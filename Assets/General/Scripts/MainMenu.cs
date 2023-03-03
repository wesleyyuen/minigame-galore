using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static void ToMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void OnRhythmButtonClicked()
    {
        SceneManager.LoadSceneAsync(MiniGameGenre.Rhythm.ToString());
    }

    public void OnTurnBasedRPGButtonClicked()
    {
        SceneManager.LoadSceneAsync(MiniGameGenre.TurnBasedRPG.ToString());
    }

    public void OnFightingButtonClicked()
    {
        SceneManager.LoadSceneAsync(MiniGameGenre.Fighting.ToString());
    }
}
