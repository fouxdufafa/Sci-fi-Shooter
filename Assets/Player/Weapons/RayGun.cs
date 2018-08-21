using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayGun : MonoBehaviour, IWeapon {

    [SerializeField] GameObject laserPrefab;
    [SerializeField] LayerMask hitLayers;
    GameObject laserBeam;
    LineRenderer laserRenderer;
    bool isFiring;
    Vector2 direction;
    PlayerAim aim;
    float maxDistance = 100f;
    Coroutine firingCoroutine;

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
    }

    public void OnFirePressed()
    {
        // get aim direction from another component
        laserBeam.SetActive(true);
        firingCoroutine = StartCoroutine(FiringCoroutine());
        Debug.Log("Start Fire!");
    }

    public void OnFireReleased()
    {
        laserBeam.SetActive(false);
        StopCoroutine(firingCoroutine);
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
