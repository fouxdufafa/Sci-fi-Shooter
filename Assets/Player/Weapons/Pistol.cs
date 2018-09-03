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
    CharacterAim aim;
    AudioSource source;

    public void OnFirePressed()
    {
        Vector2 bulletVelocity = aim.CurrentDirection * bulletSpeed;
        GameObject bullet = Instantiate(bulletPrefab, aim.WeaponSocket.transform.position, aim.transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = aim.CurrentDirection * bulletSpeed;
        source.PlayOneShot(bulletSound);
    }

    public void OnFireReleased()
    {
        return;
    }

    // Use this for initialization
    void Start () {
        aim = GameObject.FindGameObjectWithTag("Player").transform.parent.GetComponentInChildren<CharacterAim>();
        if (aim == null)
        {
            throw new MissingComponentException("Could not find CharacterAim component");
        }
        else
        {
            Debug.Log("Playeraim is not null");
        }
        source = GetComponent<AudioSource>();
	}
}
