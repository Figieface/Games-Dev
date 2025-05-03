using UnityEngine;

public class XButton : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    public void HideUI()
    {
        canvas.gameObject.SetActive(false);
    }
}
