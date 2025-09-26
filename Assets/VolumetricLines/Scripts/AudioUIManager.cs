using UnityEngine;

public class AudioUIManager : MonoBehaviour
{
    public AudioSource sfxSource; // Asignar en el inspector
    public AudioClip sonidoClick;
    public AudioClip sonidoHover;

    public void ReproducirClick()
    {
        sfxSource.PlayOneShot(sonidoClick);
    }

    public void ReproducirHover()
    {
        sfxSource.PlayOneShot(sonidoHover);
    }
}
