using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ContinuousDamage : MonoBehaviour {

    [SerializeField] int dps = 10;

    Dictionary<GameObject, Coroutine> damageRoutines = new Dictionary<GameObject, Coroutine>();

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject target = collider.gameObject;
        DamageReceiver receiver = target.GetComponent<DamageReceiver>();
        IDamageable damageable = target.GetComponent<IDamageable>();
        
        if (receiver != null || damageable != null)
        {
            Coroutine damager = StartCoroutine(DamageCoroutine(target));
            damageRoutines.Add(target, damager);
        }
    }

    IEnumerator DamageCoroutine(GameObject target)
    {
        Debug.Log("Started damage");
        DamageReceiver receiver;
        IDamageable damageable;
        while (target != null)
        {
            Debug.Log("Damaging...");
            receiver = target.GetComponent<DamageReceiver>();
            damageable = target.GetComponent<IDamageable>();
            if (receiver != null)
            {
                receiver.TakeDamage(new Damager(dps * Time.deltaTime, DamageForce.None, DamageType.Continuous));
            }
            else if (damageable != null)
            {
                damageable.TakeDamage(new Damager(dps * Time.deltaTime, DamageForce.None, DamageType.Continuous));
            }
            yield return null;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        GameObject target = collider.gameObject;
        Coroutine damager;
        bool exists = damageRoutines.TryGetValue(target, out damager);
        if (exists && damager != null)
        {
            Debug.Log("Stopped Damage");
            StopCoroutine(damager);
        }
        damageRoutines.Remove(target);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}
