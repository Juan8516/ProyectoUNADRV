using UnityEngine;

public class VRTeleportManager : MonoBehaviour
{
    [Header("Configuración de teletransporte")]
    public Camera vrCamera;
    public Transform homePoint;           // punto de inicio
    public float teleportHeightOffset = 1.6f; // altura sobre el suelo

    private Vector3 lastPosition;         // donde estabas antes de teletransportarte

    void Start()
    {
        if (vrCamera == null)
            vrCamera = Camera.main;

        if (homePoint == null)
            homePoint = transform; // posición inicial por defecto
    }

    /// <summary>
    /// Teletransporta al jugador a una posición destino.
    /// </summary>
    public void TeleportTo(Vector3 destination)
    {
        lastPosition = transform.position;

        // Mover al nuevo punto (manteniendo orientación Y)
        Vector3 target = new Vector3(destination.x, destination.y + teleportHeightOffset, destination.z);
        transform.position = target;
    }

    /// <summary>
    /// Regresa a la posición original o al punto "Home".
    /// </summary>
    public void ReturnHome()
    {
        transform.position = homePoint != null ? homePoint.position : lastPosition;
    }
}

