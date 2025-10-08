using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip backgroundClip;
    public float targetVolume = 0.5f;   // volumen final
    public float fadeDuration = 3f;     // duración del fade (en segundos)

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0f; // comienza en silencio
        audioSource.Play();
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, targetVolume, elapsed / fadeDuration);
            yield return null;
        }

        audioSource.volume = targetVolume; // asegura el volumen final
    }
}

