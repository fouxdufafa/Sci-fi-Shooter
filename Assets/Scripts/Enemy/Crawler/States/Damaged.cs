using UnityEngine;
using System.Collections;

public class Damaged : IState
{
    CrawlerController crawler;

    public Damaged(CrawlerController crawler)
    {
        this.crawler = crawler;
    }
    public void Enter()
    {
        Debug.Log("Entered Damaged state");
        crawler.flasher.Flash();
        crawler.sm.ChangeState(new Idle(crawler));
    }
    public void Exit()
    {
        
    }
    public void Update()
    {
        
    }
}
