using UnityEngine;

public class VRReturnButton : MonoBehaviour
{
    public void OnGazeSelect()
    {
        VRTeleportManager teleport = FindObjectOfType<VRTeleportManager>();
        if (teleport != null)
            teleport.ReturnHome();
    }
}

