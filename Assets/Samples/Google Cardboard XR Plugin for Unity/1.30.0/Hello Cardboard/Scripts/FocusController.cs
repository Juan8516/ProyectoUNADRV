using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class FocusController : MonoBehaviour
{
    [Header("References")]
    public Transform focusPoint;            // child of camera: where object goes
    public Transform cameraRoot;            // opcional: XR Origin (no necesario para la opción MoveObject)
    public float moveDuration = 0.7f;
    public float focusScale = 1.4f;
    public bool returnAfter = true;
    public float stayDuration = 3f;         // tiempo visible antes de volver
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    // events
    public UnityEvent onFocusStart;
    public UnityEvent onFocusComplete;
    public UnityEvent onFocusReturn;

    // internal
    Coroutine currentCoroutine;

    public void FocusOn(GameObject obj)
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(FocusRoutine(obj));
    }

    IEnumerator FocusRoutine(GameObject target)
    {
        onFocusStart?.Invoke();

        // store original state
        Transform t = target.transform;
        Vector3 origPos = t.position;
        Quaternion origRot = t.rotation;
        Vector3 origScale = t.localScale;
        Transform origParent = t.parent;
        Rigidbody rb = target.GetComponent<Rigidbody>();
        bool hadRb = rb != null;
        if (hadRb) rb.isKinematic = true;

        // move: unparent and animate to focusPoint
        t.SetParent(null, true);

        Vector3 fromPos = origPos;
        Vector3 toPos = focusPoint.position;
        Quaternion fromRot = origRot;
        // Make object face camera when focused (optional)
        Quaternion toRot = Quaternion.LookRotation((Camera.main.transform.position - toPos).normalized, Vector3.up);

        Vector3 fromScale = origScale;
        Vector3 toScale = origScale * focusScale;

        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            float p = moveCurve.Evaluate(elapsed / moveDuration);
            t.position = Vector3.Lerp(fromPos, toPos, p);
            t.rotation = Quaternion.Slerp(fromRot, toRot, p);
            t.localScale = Vector3.Lerp(fromScale, toScale, p);
            elapsed += Time.deltaTime;
            yield return null;
        }
        // final
        t.position = toPos;
        t.rotation = toRot;
        t.localScale = toScale;

        onFocusComplete?.Invoke();

        // stay a bit visible
        yield return new WaitForSeconds(stayDuration);

        // return if configured
        if (returnAfter)
        {
            elapsed = 0f;
            while (elapsed < moveDuration)
            {
                float p = moveCurve.Evaluate(elapsed / moveDuration);
                t.position = Vector3.Lerp(toPos, fromPos, p);
                t.rotation = Quaternion.Slerp(toRot, fromRot, p);
                t.localScale = Vector3.Lerp(toScale, fromScale, p);
                elapsed += Time.deltaTime;
                yield return null;
            }
            // restore parent
            t.SetParent(origParent, true);
            t.position = fromPos;
            t.rotation = fromRot;
            t.localScale = fromScale;
            if (hadRb) rb.isKinematic = false;

            onFocusReturn?.Invoke();
        }

        currentCoroutine = null;
    }
}

