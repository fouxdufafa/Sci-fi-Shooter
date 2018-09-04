using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ContinuousDamage : MonoBehaviour {

    [SerializeField] int dps = 10;

    Dictionary<GameObject, Coroutine> damageRoutines = new Dictionary<GameObject, Coroutine>();

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Started Damage");
        GameObject target = collider.gameObject;
        Coroutine damager = StartCoroutine(DamageCoroutine(target));
        damageRoutines.Add(target, damager);
    }

    IEnumerator DamageCoroutine(GameObject target)
    {
        IDamageable damageable;
        while (target != null)
        {
            Debug.Log("Damaging...");
            damageable = target.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(dps * Time.deltaTime);
                Debug.Log("Damaged!");
            }
            yield return null;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        Debug.Log("Stopped Damage");
        GameObject target = collider.gameObject;
        Coroutine damager;
        bool exists = damageRoutines.TryGetValue(target, out damager);
        if (exists && damager != null)
        {
            StopCoroutine(damager);
        }
        damageRoutines.Remove(target);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}
