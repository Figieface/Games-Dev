using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField] public Light flickeringlight;
    void Start()
    {
        InvokeRepeating("Flicker",0f,0.1f);
    }

    void Flicker()
    {
        flickeringlight.intensity = Random.Range(700,1000);
    }
}
