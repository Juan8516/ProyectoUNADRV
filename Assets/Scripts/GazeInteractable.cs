using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GazeInteractable : MonoBehaviour
{
    [TextArea]
    public string infoTexto; // Texto descriptivo que aparecerá en el panel
    public GameObject panelInfo; // Referencia al panel de información
    public Text infoText; // Texto dentro del panel
    public float zoomScale = 1.5f; // Escala de zoom
    public float zoomSpeed = 3f;

    private Vector3 originalScale;
    private bool isGazed = false;
    private bool showingInfo = false;

    void Start()
    {
        originalScale = transform.localScale;
        if (panelInfo != null)
            panelInfo.SetActive(false);
    }

    public void OnGazeEnter()
    {
        isGazed = true;
        StartCoroutine(ZoomIn());
    }

    public void OnGazeExit()
    {
        isGazed = false;
        if (!showingInfo)
            StartCoroutine(ZoomOut());
    }

    IEnumerator ZoomIn()
    {
        while (isGazed && transform.localScale.x < zoomScale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale * zoomScale, Time.deltaTime * zoomSpeed);
            yield return null;
        }

        if (isGazed && !showingInfo)
        {
            showingInfo = true;
            ShowInfo();
        }
    }

    IEnumerator ZoomOut()
    {
        while (!isGazed && transform.localScale.x > originalScale.x)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * zoomSpeed);
            yield return null;
        }
    }

    void ShowInfo()
    {
        if (panelInfo != null && infoText != null)
        {
            panelInfo.SetActive(true);
            infoText.text = infoTexto;
        }
    }

    public void CloseInfo()
    {
        showingInfo = false;
        if (panelInfo != null)
            panelInfo.SetActive(false);
        StartCoroutine(ZoomOut());
    }
}

