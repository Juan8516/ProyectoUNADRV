using UnityEngine;

public class GazeInteractable : MonoBehaviour
{
    public float tiempoActivacion = 2f;
    private float tiempoMirando = 0f;
    private bool mirando = false;

    public void Mirar()
    {
        mirando = true;
        tiempoMirando = 0f;
    }

    public void DejarDeMirar()
    {
        mirando = false;
        tiempoMirando = 0f;
    }

    void Update()
    {
        if (mirando)
        {
            tiempoMirando += Time.deltaTime;
            if (tiempoMirando >= tiempoActivacion)
            {
                Activar();
                tiempoMirando = 0f;
            }
        }
    }

    void Activar()
    {
        Debug.Log("¡Objeto activado por gaze! " + gameObject.name);
        // Aquí puedes abrir panel, mostrar info, reproducir sonido, etc.
    }
}
