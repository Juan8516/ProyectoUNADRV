using UnityEngine;

public class VRTeleportManager : MonoBehaviour
{
    [Header("Player Reference")]
    public Transform player; // Arrastra tu UnPlayer aquí desde el inspector

    [Header("Puntos de teletransporte (en orden)")]
    public Transform[] assetPoints;  // HeartPoint, BrainPoint, BonesPoint, LungsPoint
    private int currentAssetIndex = 0;

    [Header("Punto de inicio")]
    public Transform homePoint;

    void Start()
    {
        // Mover jugador al punto inicial si existe
        if (homePoint != null && player != null)
            player.position = homePoint.position;
    }

    public void TeleportToNextAsset()
    {
        currentAssetIndex++;

        // Si ya terminó todos los assets → regresar a casa
        if (currentAssetIndex >= assetPoints.Length)
        {
            Debug.Log("🎉 Recorrido completado. Regresando al inicio...");
            ReturnHome();
            return;
        }

        TeleportTo(assetPoints[currentAssetIndex].position);
    }

    public void ReturnHome()
    {
        if (homePoint != null)
            TeleportTo(homePoint.position);

        // Reiniciar recorrido
        currentAssetIndex = 0;
    }

    private void TeleportTo(Vector3 targetPosition)
    {
        if (player != null)
            player.position = targetPosition;
    }
}


