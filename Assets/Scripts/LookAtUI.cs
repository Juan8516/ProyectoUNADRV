using UnityEngine;

public class LookAtUI : MonoBehaviour
{
    public Transform target;   // Panel que la cámara debe mirar
    public float rotationSpeed = 2f;

    void Update()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0f; // Mantiene la cámara a nivel (evita inclinarse)

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
