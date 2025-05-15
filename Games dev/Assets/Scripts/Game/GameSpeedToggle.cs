using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameSpeedToggle : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI speedToggleText;

    public void Toggle()
    {
        if (Time.timeScale == 1f)
        {
            speedToggleText.text = $"x2";
            Time.timeScale = 2f;
        }
        else
        {
            speedToggleText.text = $"x1";
            Time.timeScale = 1f;
        }
    }
}
