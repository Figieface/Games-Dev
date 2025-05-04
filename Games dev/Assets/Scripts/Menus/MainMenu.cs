using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string loadMap = "map";

    public void NewGame()
    {
        //load map scene
        //SceneManager.LoadScene(loadMap);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
