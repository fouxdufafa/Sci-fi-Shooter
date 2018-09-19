using UnityEngine;
using System.Collections;

public class HurtState : IState
{
    RobotBoyCharacter character;
    Damager damager;

    public HurtState(RobotBoyCharacter character, Damager damager)
    {
        this.character = character;
        this.damager = damager;
    }

    public void Enter()
    {
        GameObject damageObject = damager.GameObject;
        Vector2 knockbackDir;
        float knockbackSpeed = 2f;
        if (damageObject.transform.position.x < character.transform.position.x)
        {
            knockbackDir = new Vector2(1, 1).normalized;
        }
        else
        {
            knockbackDir = new Vector2(-1, 1).normalized;
        }
        character.Knockback(knockbackDir, knockbackSpeed);

        character.StartCoroutine(LossOfControlRoutine());
        character.StartCoroutine(character.MakeInvulnerable(character.InvulnerableDuration));
    }

    IEnumerator LossOfControlRoutine()
    {
        yield return new WaitForSeconds(character.HurtLossOfControlDuration);
        character.sm.ChangeState(new AirborneState(character));
    }

    public void Exit()
    {
        character.SetHorizontalVelocity(0);
    }

    public void Update()
    {
        character.ApplyGravity();
        character.Move();
    }
}
