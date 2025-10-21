using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Background Music")]
    public AudioClip backgroundMusic;
    public float musicVolume = 0.5f;

    private AudioSource musicSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Crear AudioSource para música
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.volume = musicVolume;

            PlayBackgroundMusic();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
}