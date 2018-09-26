using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable {

    [SerializeField] float maxHealth;
    private float currentHealth;

    public delegate void OnHealthChanged(float newAmount);
    public event OnHealthChanged onHealthChangedObservers;

    public delegate void OnDie();
    public event OnDie onDieObservers;

    SpriteFlasher flasher;

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	public void Heal (int amount) {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        NotifyHealthChanged();
	}

    public void TakeDamage(Damager damager)
    {
        float amount = damager.Amount;
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        Debug.Log("Took " + amount + " damage, current health: " + currentHealth);
        NotifyHealthChanged();
        if (currentHealth == 0)
        {
            NotifyDeath();
        }
    }

    void NotifyDeath()
    {
        if (onDieObservers != null)
        {
            onDieObservers();
        }
    }

    void NotifyHealthChanged()
    {
        if (onHealthChangedObservers != null)
        {
            onHealthChangedObservers(currentHealth);
        }
    }
}
