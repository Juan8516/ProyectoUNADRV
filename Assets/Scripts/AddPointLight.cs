using UnityEngine;

public class AddPointLight : MonoBehaviour
{
    [Header("Configuración de la luz")]
    public Color lightColor = Color.white;
    public float intensity = 2f;
    public float range = 5f;

    void Start()
    {
        // Verifica si ya tiene una luz, para no duplicar
        if (GetComponentInChildren<Light>() == null)
        {
            GameObject lightObj = new GameObject("PointLight");
            lightObj.transform.SetParent(transform);
            lightObj.transform.localPosition = Vector3.up * 1f; // 1 unidad sobre el modelo
            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = lightColor;
            light.intensity = intensity;
            light.range = range;
            light.shadows = LightShadows.Soft;
        }
    }
}
