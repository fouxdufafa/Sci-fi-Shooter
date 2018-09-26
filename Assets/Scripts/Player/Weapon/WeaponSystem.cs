using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void OnFirePressed();
    void OnFireReleased();
}

public class WeaponSystem : MonoBehaviour, IWeapon {

    // Use this for initialization, then transfer to queue
    [SerializeField] AudioClip cycleWeaponSound;
    [SerializeField] List<GameObject> initialWeapons;

    public delegate void OnWeaponChange(IWeapon activeWeapon);
    public event OnWeaponChange onWeaponChangeListeners;

    AudioSource audioSource;

    Queue<IWeapon> weapons;
    IWeapon activeWeapon;

    public IWeapon ActiveWeapon { get { return activeWeapon; } }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        weapons = new Queue<IWeapon>();
        foreach (GameObject weapon in initialWeapons)
        {
            Component iWeapon = weapon.GetComponent(typeof(IWeapon));
            if (iWeapon)
            {
                weapons.Enqueue(Instantiate(weapon, transform).GetComponent<IWeapon>());
            }
        }
        activeWeapon = weapons.Peek();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            OnFirePressed();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            OnFireReleased();
        }
        if (Input.GetButtonDown("CycleWeapon"))
        {
            CycleWeapon();
        }
    }

    public void CycleWeapon()
    {
        activeWeapon.OnFireReleased();
        weapons.Enqueue(weapons.Dequeue());
        activeWeapon = weapons.Peek();
        audioSource.PlayOneShot(cycleWeaponSound);
        Debug.Log(activeWeapon);
        if (onWeaponChangeListeners != null)
        {
            onWeaponChangeListeners(activeWeapon);
        }
    }

    public void OnFirePressed()
    {
        activeWeapon.OnFirePressed();
    }

    public void OnFireReleased()
    {
        activeWeapon.OnFireReleased();
    }
}
