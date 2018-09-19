using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 _originalPos;
    public static CameraShake _instance;

    void Awake()
    {
        _originalPos = transform.localPosition;
    }

    public void Shake(float duration, float amount)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeRoutine(duration, amount));
    }

    private IEnumerator ShakeRoutine(float duration, float amount)
    {
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            transform.localPosition = _originalPos + (Vector3)Random.insideUnitCircle * amount;

            duration -= Time.deltaTime;

            yield return null;
        }

        transform.localPosition = _originalPos;
    }
}