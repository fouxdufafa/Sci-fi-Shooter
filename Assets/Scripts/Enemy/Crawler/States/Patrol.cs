using UnityEngine;
using System.Collections;

public class Patrol : IState
{
    CrawlerController crawler;

    public Patrol(CrawlerController crawler)
    {
        this.crawler = crawler;
    }

    public void Enter()
    {
        crawler.animator.SetBool("Walking", true);

        float delay = crawler.timeBetweenJumps;
        if (!crawler.hadFirstJump)
        {
            delay += crawler.timeInitialJumpOffset;
            crawler.hadFirstJump = true;
        }
        crawler.StartCoroutine(SwitchStates(new Jump(crawler), delay));
    }

    public void Exit()
    {
        crawler.animator.SetBool("Walking", false);
    }

    public void Update()
    {
        crawler.movement.Move();
    }

    public IEnumerator SwitchStates(IState state, float delay)
    {
        yield return new WaitForSeconds(delay);
        crawler.sm.ChangeState(state);
    }
}
