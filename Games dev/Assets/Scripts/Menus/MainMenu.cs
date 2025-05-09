using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string loadMap = "map";
    [SerializeField] private GameObject difficultyUI;
    [SerializeField] private GameObject newGameQuitUI;
    [SerializeField] private GameObject comicUI;

    public void NewGame()
    {
        newGameQuitUI.SetActive(false);
        difficultyUI.SetActive(true);
    }

    public void GoToMap()
    {
        SceneManager.LoadScene(loadMap);
    }

    public void EasyDifficulty()
    {
        DifficultyManager.gameDifficulty = 70;
        ShowComic();
    }
    public void MediumDifficulty()
    {
        DifficultyManager.gameDifficulty = 100;
        ShowComic();
    }
    public void HardDifficulty()
    {
        DifficultyManager.gameDifficulty = 140;
        ShowComic();
    }

    private void ShowComic()
    {
        comicUI.SetActive(true);
    }

    public static void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
