using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class VRGazeMovement : MonoBehaviour
{
    [Header("Configuración del movimiento")]
    public float moveSpeed = 2f;
    public Camera vrCamera;
    public bool isMoving = true;
    public float gravity = -9.8f;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (vrCamera == null)
            vrCamera = Camera.main;
    }

    void Update()
    {
        // Movimiento activado con la mirada
        if (isMoving)
        {
            Vector3 forward = vrCamera.transform.forward;
            forward.y = 0; // mantener movimiento horizontal
            forward.Normalize();

            controller.Move(forward * moveSpeed * Time.deltaTime);
        }

        // Aplicar gravedad para mantener el control sobre el suelo
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        else
            velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    // Puedes activar o desactivar el movimiento desde otros scripts
    public void StartMoving() => isMoving = true;
    public void StopMoving() => isMoving = false;
}


