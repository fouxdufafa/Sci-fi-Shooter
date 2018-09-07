using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Pistol : MonoBehaviour, IWeapon {

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 100f;
    [SerializeField] float bulletDamage;
    [SerializeField] AudioClip bulletSound;
    [SerializeField] GameObject crosshairPrefab;
    WeaponSystemV2 weaponSystem;
    AudioSource source;

    public void OnFirePressed()
    {
        Vector2 bulletVelocity = weaponSystem.AimDirection * bulletSpeed;
        GameObject bullet = Instantiate(bulletPrefab, weaponSystem.WeaponSocket.position, weaponSystem.AimAxis.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = weaponSystem.AimDirection.normalized * bulletSpeed;
        source.PlayOneShot(bulletSound);
    }

    public void OnFireReleased()
    {
        return;
    }

    // Use this for initialization
    void Start () {
        //aim = GameObject.FindGameObjectWithTag("Player").transform.parent.GetComponentInChildren<CharacterAim>();
        //if (aim == null)
        //{
        //    throw new MissingComponentException("Could not find CharacterAim component");
        //}
        //else
        //{
        //    Debug.Log("Playeraim is not null");
        //}
        weaponSystem = GameObject.FindObjectOfType<RobotBoyCharacter>().GetComponent<WeaponSystemV2>();
        source = GetComponent<AudioSource>();
	}
}
