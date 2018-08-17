using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.ThunderAndLightning;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(LightningBoltShapeSphereScript))]
public class JellyDeathEffect : MonoBehaviour {

    [SerializeField] float fadeOutTime;
    [SerializeField] AudioClip deathSound;
    AudioSource audioSource;

    LightningBoltShapeSphereScript lightning;
    float dRadius;
    float dInnerRadius;
    Coroutine fizzleOut;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        lightning = GetComponent<LightningBoltShapeSphereScript>();
        audioSource.PlayOneShot(deathSound);
        dRadius = lightning.Radius / (fadeOutTime / Time.deltaTime);
        dInnerRadius = lightning.InnerRadius / (fadeOutTime / Time.deltaTime);
        fizzleOut = StartCoroutine(FizzleOut());
	}
	
	// Update is called once per frame
	IEnumerator FizzleOut ()
    {
        while (true)
        {
            Debug.Log("Lightning radius: " + lightning.Radius);
            lightning.Radius = Mathf.Max(lightning.Radius - dRadius, 0);
            lightning.InnerRadius = Mathf.Max(lightning.InnerRadius - dInnerRadius, 0);
            yield return new WaitForEndOfFrame();
        }
    }
}
