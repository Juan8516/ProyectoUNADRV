using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class HighlightUIOnGaze : MonoBehaviour
{
    private Outline outline;
    private bool isGazedAt = false;

    [Header("Configuración del resaltado")]
    public Color highlightColor = Color.cyan;
    public float pulseSpeed = 3f;

    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false; // desactivado por defecto
    }

    void Update()
    {
        if (outline.enabled)
        {
            // Efecto de pulso dinámico en el color del borde
            float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            outline.effectColor = Color.Lerp(highlightColor * 0.5f, highlightColor, t);
        }
    }

    public void SetGazedAt(bool gazed)
    {
        isGazedAt = gazed;
        outline.enabled = gazed;
    }
}

