using UnityEngine;

public class CameraPointer : MonoBehaviour
{
    private const float _maxDistance = 10.0f;
    private GameObject _gazedAtObject = null;

    void Update()
    {
        // Detecta si la cámara está mirando a un objeto con colisionador
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance))
        {
            if (_gazedAtObject != hit.transform.gameObject)
            {
                // Se deja de mirar el objeto anterior
                if (_gazedAtObject != null)
                {
                    _gazedAtObject.SendMessage("OnPointerExit", SendMessageOptions.DontRequireReceiver);
                }

                // Nuevo objeto en la mira
                _gazedAtObject = hit.transform.gameObject;
                _gazedAtObject.SendMessage("OnPointerEnter", SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            // Si no se mira ningún objeto, notifica salida
            if (_gazedAtObject != null)
            {
                _gazedAtObject.SendMessage("OnPointerExit", SendMessageOptions.DontRequireReceiver);
                _gazedAtObject = null;
            }
        }

        // Si el usuario toca la pantalla, dispara el evento OnPointerClick
        if (Google.XR.Cardboard.Api.IsTriggerPressed)
        {
            if (_gazedAtObject != null)
            {
                _gazedAtObject.SendMessage("OnPointerClick", SendMessageOptions.DontRequireReceiver);
            }
        }

        // Actualiza parámetros de pantalla si es necesario
        Google.XR.Cardboard.Api.UpdateScreenParams();
    }
}

