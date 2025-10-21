using UnityEngine;

public class AddPointLight : MonoBehaviour
{
    [Header("Configuración de la Luz")]
    public Light pointLight;
    public float maxIntensity = 4f;
    public float minIntensity = 0.3f;
    public float pulseDuration = 0.5f;
    public Color gazeColor = Color.cyan;
    private Color originalColor;

    private void Awake()
    {
        // Si no hay una luz asignada, crear una
        if (pointLight == null)
        {
            pointLight = GetComponent<Light>();
            if (pointLight == null)
            {
                pointLight = gameObject.AddComponent<Light>();
                pointLight.type = LightType.Point;
            }
        }

        originalColor = pointLight.color;
    }

    /// <summary>
    /// Llamado cuando el jugador mantiene la mirada en un objeto.
    /// </summary>
    public void OnGazeSelect()
    {
        StopAllCoroutines();
        StartCoroutine(LightPulse());
    }

    private System.Collections.IEnumerator LightPulse()
    {
        float t = 0f;
        pointLight.color = gazeColor;

        // Incrementa intensidad
        while (t < pulseDuration)
        {
            t += Time.deltaTime;
            float value = Mathf.Lerp(minIntensity, maxIntensity, t / pulseDuration);
            pointLight.intensity = value;
            yield return null;
        }

        // Reduce intensidad
        t = 0f;
        while (t < pulseDuration)
        {
            t += Time.deltaTime;
            float value = Mathf.Lerp(maxIntensity, minIntensity, t / pulseDuration);
            pointLight.intensity = value;
            yield return null;
        }

        pointLight.color = originalColor;
    }
}

