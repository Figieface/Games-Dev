using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string loadMap = "map";
    [SerializeField] private GameObject difficultyUI;
    [SerializeField] private GameObject newGameQuitUI;
    [SerializeField] private GameObject comicUI;

    private void Start()
    {
        AudioManager.menumusicSound();
    }
    public void NewGame()
    {
        AudioManager.swordSound();
        newGameQuitUI.SetActive(false);
        difficultyUI.SetActive(true);
        ScoreManager.score = 0;
    }

    public void GoToMap()
    {
        AudioManager.swordSound();
        SceneManager.LoadScene(loadMap);
    }

    public void EasyDifficulty()
    {
        AudioManager.swordSound();
        DifficultyManager.gameDifficulty = 70;
        ShowComic();
    }
    public void MediumDifficulty()
    {
        AudioManager.swordSound();
        DifficultyManager.gameDifficulty = 100;
        ShowComic();
    }
    public void HardDifficulty()
    {
        AudioManager.swordSound();
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
        AudioManager.swordSound();
        Application.Quit();
    }
}
