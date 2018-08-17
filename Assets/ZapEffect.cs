using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCastAttack : MonoBehaviour {

    [SerializeField] GameObject aimPrefab;
    [SerializeField] GameObject firePrefab;

    GameObject zap = null;

	// Use this for initialization
	void Start () {
        zap = Instantiate(firePrefab, transform);
        this.Stop();
	}

    public void Aim (GameObject target)
    {

    }

    public void Fire()
    {
        zap.SetActive(false);
    }

    public void Stop()
    {
        zap.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
