using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAim : MonoBehaviour {

    [SerializeField] float maxDegreesDelta = 30;
    [SerializeField] GameObject crosshairPrefab;
    [SerializeField] float crosshairRadius = 5f;

    [HideInInspector]
    public Vector2 CurrentDirection { get; private set; }

    [HideInInspector]
    public Transform WeaponSocket { get; private set; }

    private Quaternion desiredRotation;
    private CharacterMovement movement;
    private WeaponSystem weaponSystem;
    private GameObject crosshair;

    void Start()
    {
        movement = transform.parent.GetComponentInChildren<CharacterMovement>();
        weaponSystem = GetComponent<WeaponSystem>();
        crosshair = Instantiate(crosshairPrefab, transform);
        crosshair.transform.localPosition = new Vector2(crosshairRadius, 0);
        crosshair.SetActive(false);
        WeaponSocket = transform.Find("WeaponSocket");
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetAxis("Left Trigger") == 1)
        {
            // Aiming
            float angle = Mathf.Rad2Deg * Mathf.Atan2(Input.GetAxis("Vertical Aim"), Input.GetAxis("Horizontal Aim"));
            desiredRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, maxDegreesDelta);
            crosshair.SetActive(true);
        }
        else
        {
            // Not aiming, set fire direction to forward
            transform.rotation = movement.IsFacingRight
                ? Quaternion.RotateTowards(transform.rotation, Quaternion.identity, 180)
                : Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 180), 180);

            crosshair.SetActive(false);
        }
        CurrentDirection = transform.right;
	}

    void OnDrawGizmos ()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 5);
    }
}
