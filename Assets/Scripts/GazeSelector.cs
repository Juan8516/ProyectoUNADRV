using UnityEngine;

public class GazeSelector : MonoBehaviour
{
    [Header("Configuración de la mirada")]
    public float gazeTime = 2f;
    private float timer;
    private GameObject gazedAtObject;

    [Header("Configuración de Zoom")]
    public Camera mainCamera;
    public float zoomFOV = 40f;
    public float zoomSpeed = 2f;

    [Header("Rotación suave hacia el objetivo")]
    public float rotationSpeed = 2f;
    private Quaternion originalRotation;
    private bool isZoomingIn = false;
    private Vector3 focusPoint;
    private bool hasFocusPoint = false;
    private float originalFOV;

    [Header("Sonidos")]
    public AudioClip zoomStartSound;
    public AudioClip selectionSound;
    private AudioSource audioSource;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera != null)
        {
            originalFOV = mainCamera.fieldOfView;
            originalRotation = mainCamera.transform.rotation;
        }

        // Crear o asignar fuente de audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 0 = sonido global (2D)
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject != gazedAtObject)
            {
                // Si se mira un nuevo objeto, reproducir sonido de zoom
                if (zoomStartSound != null)
                    audioSource.PlayOneShot(zoomStartSound);

                // Desactivar highlight anterior
                if (gazedAtObject != null)
                {
                    HighlightUIOnGaze oldHighlight = gazedAtObject.GetComponent<HighlightUIOnGaze>();
                    if (oldHighlight != null)
                        oldHighlight.SetGazedAt(false);

                    HighlightUIOnGaze oldUI = gazedAtObject.GetComponent<HighlightUIOnGaze>();
                    if (oldUI != null)
                        oldUI.SetGazedAt(false);
                }

                gazedAtObject = hit.transform.gameObject;
                timer = 0;

                // Activar highlight del nuevo objeto
                HighlightUIOnGaze newHighlight = gazedAtObject.GetComponent<HighlightUIOnGaze>();
                if (newHighlight != null)
                    newHighlight.SetGazedAt(true);

                HighlightUIOnGaze newUI = gazedAtObject.GetComponent<HighlightUIOnGaze>();
                if (newUI != null)
                    newUI.SetGazedAt(true);

                focusPoint = hit.point;
                hasFocusPoint = true;
                StartZoomIn();
            }

            timer += Time.deltaTime;
            if (timer >= gazeTime)
            {
                // 🔊 Reproducir sonido de selección
                if (selectionSound != null)
                    audioSource.PlayOneShot(selectionSound);

                // Mostrar información
                ShowInfo infoScript = gazedAtObject.GetComponent<ShowInfo>();
                if (infoScript != null)
                    infoScript.ShowInformation();

                // Efecto de luz
                AddPointLight lightEffect = gazedAtObject.GetComponent<AddPointLight>();
                if (lightEffect != null)
                    lightEffect.OnGazeSelect();

                timer = 0;
            }
        }
        else
        {
            if (gazedAtObject != null)
            {
                HighlightUIOnGaze oldHighlight = gazedAtObject.GetComponent<HighlightUIOnGaze>();
                if (oldHighlight != null)
                    oldHighlight.SetGazedAt(false);

                HighlightUIOnGaze oldUI = gazedAtObject.GetComponent<HighlightUIOnGaze>();
                if (oldUI != null)
                    oldUI.SetGazedAt(false);
            }

            StartZoomOut();
            gazedAtObject = null;
            hasFocusPoint = false;
            timer = 0;
        }

        HandleZoom();
        HandleRotation();
    }

    private void StartZoomIn() => isZoomingIn = true;
    private void StartZoomOut() => isZoomingIn = false;

    private void HandleZoom()
    {
        if (mainCamera == null) return;
        float targetFOV = isZoomingIn ? zoomFOV : originalFOV;
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }

    private void HandleRotation()
    {
        if (mainCamera == null) return;

        if (isZoomingIn && hasFocusPoint)
        {
            Vector3 direction = (focusPoint - mainCamera.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, originalRotation, Time.deltaTime * rotationSpeed);
        }
    }
}

