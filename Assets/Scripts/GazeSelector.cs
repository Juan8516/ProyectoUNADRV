using UnityEngine;

public class GazeSelector : MonoBehaviour
{
    public float gazeTime = 2f; // tiempo mirando para seleccionar
    private float timer;
    private GameObject gazedAtObject;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject != gazedAtObject)
            {
                gazedAtObject = hit.transform.gameObject;
                timer = 0;
            }

            timer += Time.deltaTime;
            if (timer >= gazeTime)
            {
                ShowInfo infoScript = gazedAtObject.GetComponent<ShowInfo>();
                if (infoScript != null)
                {
                    infoScript.ShowInformation();
                }
                timer = 0;
            }
        }
        else
        {
            gazedAtObject = null;
            timer = 0;
        }
    }
}

