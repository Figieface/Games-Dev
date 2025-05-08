using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string loadMap = "map";
    [SerializeField] private GameObject difficultyUI;
    [SerializeField] private GameObject newGameQuitUI;

    public void NewGame()
    {
        newGameQuitUI.SetActive(false);
        difficultyUI.SetActive(true);
    }

    private void GoToMap()
    {
        SceneManager.LoadScene(loadMap);
    }

    public void EasyDifficulty()
    {
        DifficultyManager.gameDifficulty = 70;
        GoToMap();
    }
    public void MediumDifficulty()
    {
        DifficultyManager.gameDifficulty = 100;
        GoToMap();
    }
    public void HardDifficulty()
    {
        DifficultyManager.gameDifficulty = 140;
        GoToMap();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
