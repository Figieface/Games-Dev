using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] private TextMeshProUGUI livesText;
    public static Lives livesInstance;
    private static int lives;

    private int startLives = 100;

    private void Awake()
    {
        livesInstance = this; //making this a singleton class
    }

    private void Start()
    {
        lives = startLives;
    }

    public void UpdateLivesText()
    {
        livesText.text = $"Lives: {lives}";
    }

    public static void DamagePlayer(int damage)
    {
        lives -= damage;
        if (lives > 0)
        {
            livesInstance.UpdateLivesText();
        }
        else
        {
            livesInstance.gameManager.EndGame();
        }
    }
}//
