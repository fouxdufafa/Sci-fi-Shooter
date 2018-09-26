using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTargetGroup : MonoBehaviour {

    [SerializeField] float spikeMovementSpeed;
    [SerializeField] float spikePauseDuration;
    [SerializeField] AudioClip disengageSound;

    Spikes spikes;
    AudioSource audioSource;

    Coroutine disengageRoutine;
    bool isDisengaged = false;

	// Use this for initialization
	void Start () {
        spikes = GetComponentInChildren<Spikes>();
        audioSource = GetComponent<AudioSource>();
	}

    public void DisengageSpikes()
    {
        if (!isDisengaged)
        {
            isDisengaged = true;
            audioSource.PlayOneShot(disengageSound);
            StartCoroutine(DisengageRoutine());
        }
    }

    IEnumerator DisengageRoutine()
    {
        // move to destination
        // pause
        // move to origin
        Vector2 origin = spikes.transform.localPosition;
        Vector2 destination = origin + spikes.BaseWidth * Vector2.left;

        yield return MoveFromTo(origin, destination);
        yield return new WaitForSeconds(spikePauseDuration);
        yield return MoveFromTo(destination, origin);

        isDisengaged = false;
    }

    IEnumerator MoveFromTo(Vector3 origin, Vector3 destination)
    {
        // use parametric function
        // x(t) = x0 * (1 - t) + x1 * t
        float startTime = Time.time;
        while (spikes.transform.localPosition != destination)
        {
            spikes.transform.localPosition = Vector3.Lerp(origin, destination, spikeMovementSpeed * (Time.time - startTime));
            yield return null;
        }
    }
}
