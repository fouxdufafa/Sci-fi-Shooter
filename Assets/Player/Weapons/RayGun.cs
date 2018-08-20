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

    // Use this for initialization
    void Start()
    {
        laserBeam = Instantiate(laserPrefab);
        laserBeam.SetActive(false);
        laserRenderer = laserBeam.GetComponent<LineRenderer>();
    }

	// Update is called once per frame
	void Update () {
		
	}

    public void OnFirePressed ()
    {
        isFiring = true;
        // get aim direction from another component
    }

    public void OnFireReleased ()
    {
        isFiring = false;
        Debug.Log("Stop Fire!");
    }
}
