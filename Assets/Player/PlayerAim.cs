﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour {

    [SerializeField] GameObject crosshairPrefab;
    [SerializeField] float crosshairDistance;

    GameObject crosshair;
    public GameObject weaponSocket;
    public Vector2 currentDirection;

    private void Start()
    {
        crosshair = Instantiate(crosshairPrefab, transform);
        weaponSocket = transform.Find("ProjectileSocket").gameObject;
    }
    public void Aim(Vector2 newDirection, bool showCrosshair)
    {
        currentDirection = newDirection.normalized;
        Vector3 offset = currentDirection * crosshairDistance;
        bool crosshairActive = showCrosshair && offset.magnitude > 0;
        crosshair.SetActive(showCrosshair && offset.magnitude > 0);
        crosshair.transform.position = transform.position + offset;
    }
}
