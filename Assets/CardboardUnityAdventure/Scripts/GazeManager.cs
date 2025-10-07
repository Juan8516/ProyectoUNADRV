using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class GazeManager : MonoBehaviour
{
    public event Action OnGazeSelection;
    public static GazeManager Instance;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private float rayDistance = 30f;
    private GameObject currentObject = null;
    private GameObject lastObject = null;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField] private GameObject gazeBarCanvas;
    [SerializeField] Image fillIndicator;
    [Tooltip("Time in seg")]
    [SerializeField] private float timeForSelection =2.5f;

    private float timeCounter;
    private float timeProggres;
    private bool runTimer;
    void Start()
    {
        gazeBarCanvas.SetActive(false);
        fillIndicator.fillAmount = Normalise();
    }


    public void Update()
    {
        
        DetectGazeObject();

        if (runTimer)
        {
            timeProggres += Time.deltaTime;
            AddValue(timeProggres);
        }
        
    }

    public void SetUpGaze(float timeForSelection) 
    {
        this.timeForSelection = timeForSelection;
    }
    public void StartGazeSelection()
    {
        gazeBarCanvas.SetActive(true);
        runTimer = true;
        timeProggres = 0;
    }

    public void CancelGazeSelection()
    {
        gazeBarCanvas.SetActive(false);
        runTimer = false;
        timeProggres = 0;
        timeCounter = 0;
    }

    private void AddValue(float val) 
    {
        timeCounter = val;
        if (timeCounter >= timeForSelection)
        {
            timeCounter = 0;
            runTimer = false;
            OnGazeSelection?.Invoke();

            if (currentObject != null)
                currentObject.SendMessage("OnGazeSelected", SendMessageOptions.DontRequireReceiver);
        }

        fillIndicator.fillAmount = Normalise();
    }
    private float Normalise() 
    {
        return (float)timeCounter / timeForSelection;
    }

    private void DetectGazeObject()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            currentObject = hit.collider.gameObject;

            if (currentObject != lastObject)
            {
                // Cuando la mirada entra a un nuevo objeto
                CancelGazeSelection(); // reinicia el temporizador
                StartGazeSelection();

                // Notifica al objeto anterior que ya no está siendo mirado
                if (lastObject != null)
                    lastObject.SendMessage("OnGazeExit", SendMessageOptions.DontRequireReceiver);

                // Notifica al nuevo objeto que está siendo mirado
                currentObject.SendMessage("OnGazeEnter", SendMessageOptions.DontRequireReceiver);

                lastObject = currentObject;
            }
        }
        else
        {
            // Si no está mirando nada, cancela el gaze
            if (lastObject != null)
                lastObject.SendMessage("OnGazeExit", SendMessageOptions.DontRequireReceiver);

            CancelGazeSelection();
            lastObject = null;
            currentObject = null;
        }
    }

}
