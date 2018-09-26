using UnityEngine;
using System.Collections;

public enum DamageForce
{
    None = 0,
    Weak,
    Average,
    Strong
}
public enum DamageType
{
    OneShot = 0,
    Continuous
}
public class Damager
{
    public float Amount { get; private set; }
    public DamageForce Force { get; private set; }
    public DamageType Type { get; private set; }
    public bool IgnoreInvincibility { get; private set; }
    public GameObject GameObject { get; private set; }

    public Damager(float amount, DamageForce force = DamageForce.Average, DamageType type = DamageType.OneShot, bool ignoreInvincibility = false, GameObject gameObject = null)
    {
        this.Amount = amount;
        this.Force = force;
        this.Type = type;
        this.IgnoreInvincibility = ignoreInvincibility;
        this.GameObject = gameObject;
    }
}
