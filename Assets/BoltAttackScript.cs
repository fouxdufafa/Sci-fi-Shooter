using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.ThunderAndLightning;

public class BoltAttackScript : MonoBehaviour {

    [SerializeField] float secondsBetweenAttacks = 3f;
    [SerializeField] float aimSeconds = 0.75f;
    [SerializeField] float fireSeconds = 1f;

    [SerializeField] GameObject target;
    [SerializeField] GameObject boltPrefab;
    [SerializeField] GameObject boltAimPrefab;
    [SerializeField] GameObject boltFirePrefab;

    [SerializeField] AudioClip boltAimSound;
    [SerializeField] AudioClip boltFireSound;
    private AudioSource audioSource;

    public delegate void OnAim();
    public event OnAim onAimObservers;

    public delegate void OnFire();
    public event OnFire onFireObservers;

    public delegate void OnCeaseFire();
    public event OnCeaseFire onCeaseFireObservers;

    Coroutine zapRoutine;
    GameObject zapBall;
    GameObject zapBolt;
    GameObject aimTarget;

    private void Start()
    {
        zapRoutine = StartCoroutine(ZapRoutine());
        aimTarget = new GameObject("AimTarget");
        audioSource = GetComponent<AudioSource>();
    }

    IEnumerator ZapRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(secondsBetweenAttacks);
            Aim();
            yield return new WaitForSeconds(aimSeconds);
            Fire();
            yield return new WaitForSeconds(fireSeconds);
            CeaseFire();
        }
    }

    void Aim()
    {
        aimTarget.transform.SetPositionAndRotation(target.transform.position, Quaternion.identity);
        zapBall = Instantiate(boltAimPrefab, aimTarget.transform.position, Quaternion.identity);
        audioSource.PlayOneShot(boltAimSound);
        if (onAimObservers != null)
        {
            onAimObservers();
        }
    }

    void Fire()
    {
        Destroy(zapBall);
        zapBall = Instantiate(boltFirePrefab, aimTarget.transform);
        zapBolt = Instantiate(boltPrefab);
        LightningBoltPrefabScript bolt = zapBolt.GetComponent<LightningBoltPrefabScript>();
        bolt.Source = gameObject;
        bolt.Destination = aimTarget;
        audioSource.loop = true;
        audioSource.PlayOneShot(boltFireSound);
        if (onFireObservers != null)
        {
            onFireObservers();
        }
    }

    void CeaseFire()
    {
        Destroy(zapBall);
        Destroy(zapBolt);
        audioSource.Stop();
        if (onCeaseFireObservers != null)
        {
            onCeaseFireObservers();
        }
    }

    private void OnDestroy()
    {
        Destroy(zapBall);
        Destroy(zapBolt);
        audioSource.Stop();
        StopCoroutine(zapRoutine);
    }
}
