using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class CrawlerController : MonoBehaviour, IDamageable {

    public CollisionAwareStateMachine sm { get; private set; }
    public Animator animator { get; private set; }
    public SpriteFlasher flasher { get; private set; }
    public AudioSource audioSource { get; private set; }
    public PatrolMovement movement { get; private set; }

    [SerializeField] AudioClip damageSound;

    float lastContinuousDamageFlashTime = 0f;
    float lastContinuousDamageSoundTime = 0f;
    float continuousDamageFlashTimeThreshold = 0.25f;
    float continuousDamageSoundTimeThreshold = 1f;

    // Use this for initialization
    void Start () {
        movement = GetComponent<PatrolMovement>();
        animator = GetComponent<Animator>();
        flasher = GetComponent<SpriteFlasher>();
        audioSource = GetComponent<AudioSource>();

        sm = new CollisionAwareStateMachine();
        sm.ChangeState(new Patrol(this));
	}
	
	// Update is called once per frame
	void Update () {
        sm.Update();
	}

    public void TakeDamage(Damager damager)
    {
        if (damager.Type == DamageType.Continuous)
        {
            if (Time.time - lastContinuousDamageFlashTime > continuousDamageFlashTimeThreshold)
            {
                flasher.Flash();
                lastContinuousDamageFlashTime = Time.time;
            }
            if (Time.time - lastContinuousDamageSoundTime > continuousDamageSoundTimeThreshold)
            {
                audioSource.PlayOneShot(damageSound);
                lastContinuousDamageSoundTime = Time.time;
            }
        }

        if (damager.Type == DamageType.OneShot)
        {
            flasher.Flash();
            audioSource.PlayOneShot(damageSound);
        }
    }
}
