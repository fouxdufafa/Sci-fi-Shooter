using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayGun : MonoBehaviour, IWeapon {

    [SerializeField] GameObject laserPrefab;
    [SerializeField] LayerMask hitLayers;
    [SerializeField] AudioClip laserSoundLoop;
    GameObject laserBeam;
    LineRenderer laserRenderer;
    bool isFiring;
    Vector2 direction;
    PlayerAim aim;
    float maxDistance = 100f;
    Coroutine firingCoroutine;
    AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        aim = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAim>();
        if (aim == null)
        {
            throw new MissingComponentException("Could not find playeraim component");
        }
        laserBeam = Instantiate(laserPrefab);
        laserBeam.SetActive(false);
        laserRenderer = laserBeam.GetComponent<LineRenderer>();

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = laserSoundLoop;
        audioSource.loop = true;
    }

    public void OnFirePressed()
    {
        // get aim direction from another component
        laserBeam.SetActive(true);
        firingCoroutine = StartCoroutine(FiringCoroutine());
        Debug.Log("Start Fire!");
        audioSource.Play();
    }

    public void OnFireReleased()
    {
        laserBeam.SetActive(false);
        if (firingCoroutine != null) 
        {
            // Safeguards the scenario in which weapon switch calls OnFireReleased without having ever fired this weapon
            // Perhaps weapons should have an OnSelect and OnDeselect behavior in the interface instead
            StopCoroutine(firingCoroutine);
        }
        audioSource.Stop();
        Debug.Log("Stop Fire!");
    }

    IEnumerator FiringCoroutine()
    {
        while (true)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, aim.currentDirection, maxDistance, hitLayers);
            Vector2 origin = transform.position;
            Vector2 destination;
            if (raycastHit.collider != null)
            {
                destination = raycastHit.point;
            }
            else
            {
                destination = origin + aim.currentDirection * maxDistance;
            }
            laserRenderer.SetPositions(new Vector3[] { origin, destination });
            yield return null;
        }
    }
}
