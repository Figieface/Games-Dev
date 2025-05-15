using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool gameIsOver;
    public GameObject gameOverUI;
    [SerializeField] public TextMeshProUGUI scoreUI;

    private void Start()
    {
        gameIsOver = false;
    }
    public void EndGame()
    {
        gameOverUI.SetActive(true);
        scoreUI.text = $"Score: {ScoreManager.score}";
        Debug.Log("Game Over!");
    }

    public void BackToMap()
    {
        SceneManager.LoadScene("Map");
    }
    
}
