using UnityEngine;
using System.Collections;

public enum DamageForce
{
    None = 0,
    Weak,
    Average,
    Strong
}
public class Damager
{
    public float Amount { get; private set; }
    public DamageForce Force { get; private set; }
    public bool IgnoreInvincibility { get; private set; }

    public Damager(float amount, DamageForce force = DamageForce.Average, bool ignoreInvincibility = false)
    {
        this.Amount = amount;
        this.Force = force;
        this.IgnoreInvincibility = ignoreInvincibility;
    }
}
