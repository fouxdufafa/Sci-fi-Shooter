using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Pistol : MonoBehaviour, IWeapon {

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 100f;
    [SerializeField] float bulletDamage;
    [SerializeField] AudioClip bulletSound;
    PlayerAim aim;
    AudioSource source;

    public void OnFirePressed()
    {
        Debug.Log(aim);
        Vector2 bulletVelocity = aim.currentDirection * bulletSpeed;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.FromToRotation(Vector2.right, aim.currentDirection));
        bullet.GetComponent<Rigidbody2D>().velocity = aim.currentDirection * bulletSpeed;
        source.PlayOneShot(bulletSound);
    }

    public void OnFireReleased()
    {
        return;
    }

    // Use this for initialization
    void Start () {
        aim = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAim>();
        if (aim == null)
        {
            throw new MissingComponentException("Could not find playeraim component");
        }
        else
        {
            Debug.Log("Playeraim is not null");
        }
        source = GetComponent<AudioSource>();
	}
}
