using UnityEngine;
using TMPro;
using System.Collections;

public class ShowInfo : MonoBehaviour
{
    [TextArea]
    public string info; // texto a mostrar
    public GameObject infoPanel; // panel o canvas del texto
    public TextMeshProUGUI infoText; // referencia al texto del panel
    public float displayTime = 3f; // tiempo visible (en segundos)

    public void ShowInformation()
    {
        infoPanel.SetActive(true);
        infoText.text = info;
        StopAllCoroutines(); // por si ya estaba corriendo otra
        StartCoroutine(HideAfterSeconds());
        FindObjectOfType<QuizManager>().StartQuestion(gameObject.tag);
    }

    private IEnumerator HideAfterSeconds()
    {
        yield return new WaitForSeconds(displayTime);
        infoPanel.SetActive(false);
    }
}

