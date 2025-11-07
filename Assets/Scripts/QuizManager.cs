using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class QuestionData
    {
        public string assetName;
        [TextArea] public string questionText;
        public string[] options = new string[4];
        public int correctIndex;
    }

    public QuestionData[] questions;

    [Header("UI")]
    public GameObject questionPanel;
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;

    [Header("Puntaje")]
    public int score = 0;
    private int attempts = 0;

    private QuestionData currentQuestion;

    void Start()
    {
        questionPanel.SetActive(false);
    }

    public void StartQuestion(string assetName)
    {
        currentQuestion = GetQuestion(assetName);

        if (currentQuestion == null)
        {
            Debug.LogWarning("No se encontró pregunta para " + assetName);
            return;
        }

        StartCoroutine(ShowQuestionAfterDelay(30f));
    }

    IEnumerator ShowQuestionAfterDelay(float t)
    {
        yield return new WaitForSeconds(t);
        ShowQuestion();
    }

    void ShowQuestion()
    {
        // Obtener cámara y ubicar panel frente al jugador
        Transform cam = Camera.main.transform;
        StartCoroutine(MovePanelToFront());


        questionPanel.SetActive(true);
        attempts = 0;

        questionText.text = currentQuestion.questionText;

        for (int i = 0; i < 4; i++)
        {
            int index = i;
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.options[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => Answer(index));
        }

        FindObjectOfType<LookAtUI>().target = questionPanel.transform;
        FindObjectOfType<LookAtUI>().target = null;
    }

    void Answer(int index)
    {
        if (index == currentQuestion.correctIndex)
        {
            score += 10;
            Debug.Log("✅ Correcto. Puntos: " + score);
            questionPanel.SetActive(false);
            FindObjectOfType<LookAtUI>().target = null;
            FindObjectOfType<VRTeleportManager>().TeleportToNextAsset();
        }
        else
        {
            attempts++;
            if (attempts >= 2)
            {
                questionText.text = "❌ Incorrecto. Vuelve a intentarlo.";
            }
            else
            {
                questionText.text = "⚠️ Incorrecto, intenta de nuevo.";
            }
        }
    }

    QuestionData GetQuestion(string assetName)
    {
        foreach (var q in questions)
        {
            if (q.assetName.ToLower() == assetName.ToLower())
                return q;
        }
        return null;
    }

    IEnumerator MovePanelToFront()
    {
        Transform cam = Camera.main.transform;

        Vector3 targetPosition = cam.position + cam.forward * 1.8f;
        Quaternion targetRotation = Quaternion.LookRotation(cam.forward);

        float elapsed = 0f;
        float duration = 0.6f; // tiempo del movimiento suave

        Vector3 startPos = questionPanel.transform.position;
        Quaternion startRot = questionPanel.transform.rotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            questionPanel.transform.position = Vector3.Lerp(startPos, targetPosition, t);
            questionPanel.transform.rotation = Quaternion.Slerp(startRot, targetRotation, t);

            yield return null;
        }

        // asegurar posición final
        questionPanel.transform.position = targetPosition;
        questionPanel.transform.rotation = targetRotation;
    }

}

