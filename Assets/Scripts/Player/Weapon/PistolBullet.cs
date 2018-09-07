using UnityEngine;
using System.Collections;

public class PistolBullet : MonoBehaviour
{
    public float damage = 10f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet hit trigger collider");
        Damageable damageable = collision.gameObject.GetComponent<Damageable>();
        if (damageable != null)
        {
            Debug.Log("Hit an IDamageable");
            damageable.TakeDamage(damage);
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
