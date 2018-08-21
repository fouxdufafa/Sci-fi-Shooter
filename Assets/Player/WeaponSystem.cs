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
    [SerializeField] List<GameObject> initialWeapons;

    public delegate void OnWeaponChange(IWeapon activeWeapon);
    public event OnWeaponChange onWeaponChangeListeners;

    Queue<IWeapon> weapons;
    IWeapon activeWeapon;

    private void Start()
    {
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

    public void CycleWeapon()
    {
        activeWeapon.OnFireReleased();
        weapons.Enqueue(weapons.Dequeue());
        activeWeapon = weapons.Peek();
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
