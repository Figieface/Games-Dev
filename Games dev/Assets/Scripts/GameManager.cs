using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameIsOver;
    public GameObject gameOverUI;

    private void Start()
    {
        gameIsOver = false;
    }
    public void EndGame()
    {
        gameOverUI.SetActive(true);
        Debug.Log("Game Over!");
    }
}
