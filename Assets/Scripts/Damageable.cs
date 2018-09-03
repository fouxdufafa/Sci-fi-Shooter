using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable {

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
        flasher = GetComponent<SpriteFlasher>();
	}
	
	// Update is called once per frame
	public void Heal (int amount) {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (onHealthChangedObservers != null)
        {
            onHealthChangedObservers(currentHealth);
        }
	}

    public void TakeDamage (float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        Debug.Log("Took " + amount + " damage, current health: " + currentHealth);
        flasher.Flash();
        if (currentHealth == 0)
        {
            if (onDieObservers != null)
            {
                Debug.Log("Called OnDie for " + gameObject.name);
                onDieObservers();
            }
        }
    }
}
