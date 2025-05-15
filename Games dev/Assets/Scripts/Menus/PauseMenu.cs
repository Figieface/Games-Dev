using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI;
    [SerializeField]public TextMeshProUGUI scoreUI;

    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        scoreUI.text = $"Score: {ScoreManager.score}";
        pauseUI.SetActive(!pauseUI.activeSelf);
        if (pauseUI.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void mainMenu()
    {
        MainMenu.GoToMainMenu();
    }
}
