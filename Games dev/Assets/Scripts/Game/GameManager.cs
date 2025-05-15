using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool gameIsOver;
    public GameObject gameOverUI;

    public static int difficultyScore;

    private void Start()
    {
        gameIsOver = false;
    }
    public void EndGame()
    {
        gameOverUI.SetActive(true);
        Debug.Log("Game Over!");
    }

    public void BackToMap()
    {
        SceneManager.LoadScene("Map");
    }
    
}
