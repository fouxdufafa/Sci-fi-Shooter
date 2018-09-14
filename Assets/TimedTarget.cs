using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedTarget : MonoBehaviour, IDamageable {

    [SerializeField] float secondsUntilRevert = 1.5f;
    [SerializeField] Sprite lockedSprite;
    [SerializeField] Sprite pendingSprite;
    [SerializeField] Sprite unlockedSprite;

    public enum LockState { Locked, Pending, Unlocked }

    public delegate void OnStateChange(TimedTarget t, LockState state);
    public event OnStateChange onStateChangeListeners;

    LockState currentState;
    SpriteRenderer spriteRenderer;

    void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeState(LockState.Locked);
    }

    public void TakeDamage(Damager damager)
    {
        switch (currentState)
        {
            case LockState.Locked:
                ChangeState(LockState.Pending);
                break;
            case LockState.Pending:
                break;
            case LockState.Unlocked:
                break;
        }
    }

    public void Unlock()
    {
        ChangeState(LockState.Unlocked);
    }

    void ChangeState(LockState state)
    {
        switch (state)
        {
            case LockState.Pending:
                spriteRenderer.sprite = pendingSprite;
                StartCoroutine(PendingStateCoroutine());
                break;
            case LockState.Locked:
                spriteRenderer.sprite = lockedSprite;
                break;
            case LockState.Unlocked:
                spriteRenderer.sprite = unlockedSprite;
                break;
        }
        currentState = state;
        if (onStateChangeListeners != null)
        {
            onStateChangeListeners(this, currentState);
        }
    }

    IEnumerator PendingStateCoroutine ()
    {
        yield return new WaitForSeconds(secondsUntilRevert);
        if (currentState != LockState.Unlocked)
        {
            ChangeState(LockState.Locked);
        }
    }
}
