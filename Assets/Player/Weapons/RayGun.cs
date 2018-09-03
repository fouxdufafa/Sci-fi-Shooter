using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayGun : MonoBehaviour, IWeapon {

    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject damagePrefab;
    [SerializeField] LayerMask hitLayers;
    [SerializeField] AudioClip laserSoundLoop;
    [SerializeField] float maxAngleBetweenRaycasts = 10f; // Used to cover laser "sweeps" with intermediate raycasts

    GameObject laserBeam;
    LineRenderer laserRenderer;
    bool isFiring;
    CharacterAim aim;
    float maxDistance = 100f;
    Coroutine firingCoroutine;
    AudioSource audioSource;
    GameObject damager;

    // Use this for initialization
    void Start()
    {
        aim = GameObject.FindGameObjectWithTag("Player").transform.parent.GetComponentInChildren<CharacterAim>();
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

        damager = Instantiate(damagePrefab);
        damager.SetActive(false);
    }

    public void OnFirePressed()
    {
        // get aim direction from another component
        laserBeam.SetActive(true);
        damager.SetActive(true);
        firingCoroutine = StartCoroutine(FiringCoroutine());
        audioSource.Play();
    }

    public void OnFireReleased()
    {
        laserBeam.SetActive(false);
        damager.SetActive(false);
        if (firingCoroutine != null) 
        {
            // Safeguards the scenario in which weapon switch calls OnFireReleased without having ever fired this weapon
            // Perhaps weapons should have an OnSelect and OnDeselect behavior in the interface instead
            StopCoroutine(firingCoroutine);
        }
        audioSource.Stop();
    }

    IEnumerator FiringCoroutine()
    {
        Vector2 currentDirection = aim.CurrentDirection;
        Vector2 previousDirection = aim.CurrentDirection;

        while (true)
        {
            currentDirection = aim.CurrentDirection;
            Vector2.Lerp(previousDirection, currentDirection, 0.1f);

            // Doesn't account for change in player position, but this should be negligible
            float angleApprox = Vector2.Angle(previousDirection, currentDirection);
            float numRaycasts = Mathf.RoundToInt(angleApprox / maxAngleBetweenRaycasts);

            List<Ray2D> rays = new List<Ray2D>();
            for (int i = 0; i < numRaycasts; i++)
            {
                Vector2 direction = Vector2.Lerp(previousDirection, currentDirection, i / (float) numRaycasts);
            }

            Vector2 origin = aim.WeaponSocket.transform.position;
            RaycastHit2D raycastHit = Physics2D.Raycast(origin, aim.CurrentDirection, maxDistance, hitLayers);
            Vector2 destination;
            if (raycastHit.collider != null)
            {
                destination = raycastHit.point;
            }
            else
            {
                destination = origin + aim.CurrentDirection * maxDistance;
            }
            laserRenderer.SetPositions(new Vector3[] { origin, destination });
            damager.transform.position = destination;

            previousDirection = currentDirection;
            yield return null;
        }
    }
}
