using UnityEngine;

public class GazeSelector : MonoBehaviour
{
    public float gazeTime = 2f; // tiempo mirando para seleccionar
    private float timer;
    private GameObject gazedAtObject;

    [Header("Zoom Settings")]
    public Camera mainCamera;
    public float zoomFOV = 40f; // FOV reducido (más zoom)
    public float zoomSpeed = 2f; // rapidez del cambio
    private float originalFOV;
    private bool isZoomingIn = false;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera != null)
            originalFOV = mainCamera.fieldOfView;
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject != gazedAtObject)
            {
                // Nuevo objeto bajo la mirada
                gazedAtObject = hit.transform.gameObject;
                timer = 0;
                StartZoomIn(); // empezar zoom
            }

            timer += Time.deltaTime;
            if (timer >= gazeTime)
            {
                ShowInfo infoScript = gazedAtObject.GetComponent<ShowInfo>();
                if (infoScript != null)
                    infoScript.ShowInformation();

                AddPointLight lightEffect = gazedAtObject.GetComponent<AddPointLight>();
                if (lightEffect != null)
                    lightEffect.OnGazeSelect();

                timer = 0;
            }
        }
        else
        {
            // Nada en la mira, restablecer zoom
            if (gazedAtObject != null)
                StartZoomOut();

            gazedAtObject = null;
            timer = 0;
        }

        HandleZoom();
    }

    private void StartZoomIn()
    {
        isZoomingIn = true;
    }

    private void StartZoomOut()
    {
        isZoomingIn = false;
    }

    private void HandleZoom()
    {
        if (mainCamera == null) return;

        float targetFOV = isZoomingIn ? zoomFOV : originalFOV;
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }
}
