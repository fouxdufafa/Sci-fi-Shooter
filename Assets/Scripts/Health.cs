using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable {

    [SerializeField] int maxHealth;
    private int currentHealth;

    public delegate void OnHealthChanged(int newAmount);
    public event OnHealthChanged onHealthChangedObservers;

    public delegate void OnDie();
    public event OnDie onDieObservers;

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	public void Heal (int amount) {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (onHealthChangedObservers != null)
        {
            onHealthChangedObservers(currentHealth);
        }
	}

    public void TakeDamage (int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        Debug.Log("Took " + amount + " damage, current health: " + currentHealth);
        if (currentHealth == 0)
        {
            if (onDieObservers != null)
            {
                onDieObservers();
            }
        }
    }
}
