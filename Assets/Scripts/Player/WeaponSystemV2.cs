using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponSystemV2 : MonoBehaviour
{
    [SerializeField] AudioClip cycleWeaponSound;
    [SerializeField] List<GameObject> initialWeapons;
    [SerializeField] float maxDegreesDelta = 30;
    [SerializeField] GameObject crosshairPrefab;
    [SerializeField] float crosshairRadius = 5f;
    [SerializeField] float weaponSocketRadius = 1f;

    public delegate void OnWeaponChange(IWeapon activeWeapon);
    public event OnWeaponChange onWeaponChangeListeners;

    AudioSource audioSource;

    Queue<IWeapon> weapons;
    public IWeapon ActiveWeapon { get; protected set; }

    [HideInInspector]
    public Vector2 AimDirection;

    GameObject crosshair;
    Transform AimAxis;
    Transform WeaponSocket;

    private void Start()
    {
        InitializeWeapons();

        // Aim axis
        GameObject axis = new GameObject("AimAxis");
        axis.transform.parent = transform;
        axis.transform.localPosition = Vector2.zero;
        AimAxis = axis.transform;

        // Crosshair
        crosshair = Instantiate(crosshairPrefab, AimAxis);
        crosshair.transform.localPosition = new Vector2(crosshairRadius, 0);
        crosshair.SetActive(false);

        // Weapon socket
        GameObject socket = new GameObject("WeaponSocket");
        socket.transform.parent = AimAxis;
        socket.transform.localPosition = new Vector2(weaponSocketRadius, 0);
        WeaponSocket = socket.transform;

        audioSource = GetComponent<AudioSource>();
    }

    void InitializeWeapons()
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
        ActiveWeapon = weapons.Peek();
    }

    public void OnFirePressed()
    {
        ActiveWeapon.OnFirePressed();
    }

    public void OnFireReleased()
    {
        ActiveWeapon.OnFireReleased();
    }

    public void CycleWeapon()
    {
        ActiveWeapon.OnFireReleased();
        weapons.Enqueue(weapons.Dequeue());
        ActiveWeapon = weapons.Peek();
        audioSource.PlayOneShot(cycleWeaponSound);
        Debug.Log(ActiveWeapon);
        if (onWeaponChangeListeners != null)
        {
            onWeaponChangeListeners(ActiveWeapon);
        }
    }

    public void SetAimDirection(Vector2 direction)
    {
        // TODO: Lerp this
        AimAxis.localScale = new Vector3(Mathf.Abs(AimAxis.localScale.x) * Mathf.Sign(transform.localScale.x), AimAxis.localScale.y, AimAxis.localScale.z);
        float angle = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
        Quaternion desiredRotation = Quaternion.Euler(0, 0, angle);
        AimAxis.rotation = Quaternion.RotateTowards(AimAxis.rotation, desiredRotation, 360);
    }

    public void EnableCrosshair()
    {
        crosshair.SetActive(true);
    }

    public void DisableCrosshair()
    {
        crosshair.SetActive(false);
    }
}
