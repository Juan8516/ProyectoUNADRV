using UnityEngine;

public class TouchToFocus : MonoBehaviour
{
    public Camera mainCamera;                // asignar Camera.main en inspector
    public LayerMask interactableLayer;
    public FocusController focusController;

    void Awake()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    void Update()
    {
        // Touch (mobile)
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                TryTouchAt(t.position);
            }
        }
        // Mouse (Editor testing)
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            TryTouchAt(Input.mousePosition);
        }
#endif
    }

    void TryTouchAt(Vector2 screenPos)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, interactableLayer))
        {
            focusController.FocusOn(hit.transform.gameObject);
        }
    }
}

