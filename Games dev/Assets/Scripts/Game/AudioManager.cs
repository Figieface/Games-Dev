using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip arrow;
    [SerializeField] AudioClip cannon;
    [SerializeField] AudioClip sword;
    [SerializeField] AudioClip construct;
    [SerializeField] AudioClip demolish;
    [SerializeField] AudioClip denied;
    [SerializeField] AudioClip lost;
    [SerializeField] AudioClip won;
    [SerializeField] AudioClip magic;
    [SerializeField] AudioClip menumusic;
    [SerializeField] AudioClip gamemusic;
    [SerializeField] AudioClip startround;

    public static AudioManager AMInstance;

    private void Awake()
    {
        if (AMInstance == null) //setting up class to be singleton
        {
            AMInstance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip, float volume)
    {
        GameObject tempAudioSource = new GameObject("TempAudioSource");
        tempAudioSource.transform.position = Camera.main.transform.position;

        AudioSource audioSource = tempAudioSource.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        Destroy(tempAudioSource, clip.length); // destroy when done
    }

    public void PlaySoundLooped(AudioClip clip, float volume)
    {
        GameObject tempAudioSource = new GameObject("TempAudioSource");
        tempAudioSource.transform.position = Camera.main.transform.position;

        AudioSource audioSource = tempAudioSource.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    public static void arrowSound()
    {
        AMInstance.PlaySound(AMInstance.arrow, 0.2f);
    }
    public static void cannonSound()
    {
        AMInstance.PlaySound(AMInstance.cannon, 1.2f);
    }
    public static void swordSound()
    {
        AMInstance.PlaySound(AMInstance.sword, 0.8f);
    }
    public static void constructSound()
    {
        AMInstance.PlaySound(AMInstance.construct, 1f);
    }
    public static void demolishSound()
    {
        AMInstance.PlaySound(AMInstance.demolish, 0.1f);
    }
    public static void deniedSound()
    {
        AMInstance.PlaySound(AMInstance.denied, 1f);
    }
    public static void lostSound()
    {
        AMInstance.PlaySound(AMInstance.lost, 1f);
    }
    public static void wonSound()
    {
        AMInstance.PlaySound(AMInstance.won, 1f);
    }
    public static void magicSound()
    {
        AMInstance.PlaySound(AMInstance.magic, 0.3f);
    }
    public static void menumusicSound()
    {
        AMInstance.PlaySoundLooped(AMInstance.menumusic, 0.1f);
    }
    public static void gamemusicSound()
    {
        AMInstance.PlaySoundLooped(AMInstance.gamemusic, 0.2f);
    }
    public static void startroundSound()
    {
        AMInstance.PlaySound(AMInstance.startround, 0.5f);
    }
}
