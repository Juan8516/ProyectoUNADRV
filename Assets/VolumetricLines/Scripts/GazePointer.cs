using UnityEngine;
using UnityEngine.EventSystems;

public class GazePointer : MonoBehaviour
{
    public float rayLength = 10f;
    private GameObject objetoActual;

    void Update()
    {
        Ray rayo = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(rayo, out hit, rayLength))
        {
            GameObject objetoDetectado = hit.collider.gameObject;

            if (objetoActual != objetoDetectado)
            {
                if (objetoActual != null)
                    objetoActual.SendMessage("DejarDeMirar", SendMessageOptions.DontRequireReceiver);

                objetoActual = objetoDetectado;
                objetoActual.SendMessage("Mirar", SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            if (objetoActual != null)
            {
                objetoActual.SendMessage("DejarDeMirar", SendMessageOptions.DontRequireReceiver);
                objetoActual = null;
            }
        }
    }
}
