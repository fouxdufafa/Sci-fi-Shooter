using UnityEngine;
using System.Collections;

public class PistolBullet : MonoBehaviour
{
    public float damage = 10f;
    public float destroyAfterSeconds = 1f;

    Coroutine destroyRoutine;

    private void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet hit trigger collider");
        DamageReceiver receiver = collision.gameObject.GetComponent<DamageReceiver>();
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (receiver != null) 
        {
            Debug.Log("Hit a DamageReceiver");
            receiver.TakeDamage(new Damager(damage));
        }
        else if (damageable != null)
        {
            Debug.Log("Hit an IDamageable");
            damageable.TakeDamage(new Damager(damage));
        }
        StartCoroutine(DestroyAtEndOfFrame());
    }

    IEnumerator DestroyAtEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("Destroying bullet");
        Destroy(gameObject);
    }
}
