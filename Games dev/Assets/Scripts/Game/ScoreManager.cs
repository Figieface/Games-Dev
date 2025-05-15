using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int score;
    [SerializeField] public TextMeshProUGUI scoretext;

    private void Update()
    {
        scoretext.text = $"Score: {score}";
    }
}
