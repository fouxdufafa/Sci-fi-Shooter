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
    }

    public void Exit()
    {
        crawler.animator.SetBool("Walking", false);
    }

    public void Update()
    {
        crawler.movement.Move();
    }
}
