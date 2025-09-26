using UnityEngine;

public class SonidoInteraccion : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoClick;

    public void ReproducirSonido()
    {
        audioSource.PlayOneShot(sonidoClick);
    }
}

