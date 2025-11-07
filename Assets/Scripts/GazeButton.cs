using UnityEngine;
using UnityEngine.UI;

public class GazeButton : MonoBehaviour
{
    public float gazeTime = 2f;
    private float timer;
    private bool gazedAt = false;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        if (gazedAt)
        {
            timer += Time.deltaTime;
            if (timer >= gazeTime)
            {
                button.onClick.Invoke();
                timer = 0f;
            }
        }
    }

    public void OnPointerEnter()
    {
        gazedAt = true;
    }

    public void OnPointerExit()
    {
        gazedAt = false;
        timer = 0f;
    }
}

