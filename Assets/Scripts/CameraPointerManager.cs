using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Management;

public class CameraPointerManager : MonoBehaviour
{
    [SerializeField] private GameObject pointer;
    [SerializeField] private float maxDistancePointer = 4.5f;

    [Range(0, 1)]
    [SerializeField] private float disPointerObject = 0.95f;

    private const float _maxDistance = 10.0f;
    private GameObject _gazedAtObject = null;

    private readonly string interactableTag = "Interactable";
    private float scaleSize = 0.025f;


    private void Start()
    {
        GazeManager.Instance.OnGazeSelection += GazeSelection;
    }

    private void GazeSelection()
    {
        _gazedAtObject.SendMessage("OnPointerClick", null, SendMessageOptions.DontRequireReceiver);
    }

    private void Instance_OnGazeSelection()
    {
        throw new System.NotImplementedException();
    }

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
                    _gazedAtObject.SendMessage("OnPointerExit", null, SendMessageOptions.DontRequireReceiver);
                }

                // Nuevo objeto en la mira
                _gazedAtObject = hit.transform.gameObject;
                _gazedAtObject.SendMessage("OnPointerEnter", null, SendMessageOptions.DontRequireReceiver);
                GazeManager.Instance.StartGazeSelection();
            }
            if(hit.transform.CompareTag(interactableTag))
            {
                PointerOnGaze(hit.point);
            }
            else
            {
                PointerOutGaze();
            }
        }
        else
        {
            // Si no se mira ningún objeto, notifica salida
            if (_gazedAtObject != null)
            {
                _gazedAtObject.SendMessage("OnPointerExit", null, SendMessageOptions.DontRequireReceiver);
                _gazedAtObject = null;
            }
        }

        // Si el usuario toca la pantalla, dispara el evento OnPointerClick
        if (Google.XR.Cardboard.Api.IsTriggerPressed)
        {
            if (_gazedAtObject != null)
            {
                _gazedAtObject.SendMessage("OnPointerClick", null, SendMessageOptions.DontRequireReceiver);
            }
        }

        // Actualiza parámetros de pantalla si es necesario
        if (XRGeneralSettings.Instance != null &&
        XRGeneralSettings.Instance.Manager != null &&
        XRGeneralSettings.Instance.Manager.activeLoader != null)
        {
            Google.XR.Cardboard.Api.UpdateScreenParams();
        }
    }

    private void PointerOutGaze()
    {
        pointer.transform.localScale = Vector3.one * 0.1f;
        pointer.transform.parent.transform.localPosition = new Vector3(0, 0, maxDistancePointer);
        pointer.transform.parent.parent.transform.rotation = transform.rotation;
        GazeManager.Instance.CancelGazeSelection();
    }

    private void PointerOnGaze(Vector3 hitpoint)
    {
        float scaleFactor = scaleSize * Vector3.Distance(transform.position, hitpoint);
        pointer.transform.localScale = Vector3.one * scaleFactor;
        pointer.transform.parent.position = CalculatePointerPosition(transform.position, hitpoint, disPointerObject);
    }

    private Vector3 CalculatePointerPosition(Vector3 p0, Vector3 p1, float t)
    {
        float x = p0.x = t * (p1.x - p0.x);
        float y = p0.y = t * (p1.y - p0.y);
        float z = p0.z = t * (p1.z - p0.z);

        return new Vector3(x, y, z);
    }
}
