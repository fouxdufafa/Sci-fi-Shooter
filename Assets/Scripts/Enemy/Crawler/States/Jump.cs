using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Jump : IState, ICollisionAware
{
    public float jumpDuration = 0.5f;

    float startTime;
    float endTime;

    float maxRaycastDistance = 20f;

    // Points along bezier arc of jump
    Vector3 p0;
    Vector3 p1;
    Vector3 p2;

    RaycastHit2D closestSurface;

    // Rotations
    Quaternion originRotation;
    Quaternion destRotation;

    CrawlerController crawler;

    public Jump(CrawlerController crawler)
    {
        this.crawler = crawler;
    }

    public void Enter()
    {
        startTime = Time.time;
        endTime = startTime + jumpDuration;
        // calculate arc
        // p0 = current transform, 
        // p1 = player transform + height(collider)/2, 
        // p2 = raycast to find closest surface to player
        closestSurface = FindClosestSurfaceTo(crawler.player.transform.position);

        p0 = crawler.transform.position;
        p2 = AddColliderPadding(closestSurface, crawler.GetComponent<BoxCollider2D>());
        p1 = Vector2.Lerp(p0, p2, 0.5f);

        crawler.animator.enabled = false;
    }

    Vector3 AddColliderPadding(RaycastHit2D surfaceHit, BoxCollider2D collider)
    {
        Vector3 normal = surfaceHit.normal.normalized;
        Vector3 tangent = Vector3.Cross(normal, Vector3.forward).normalized;
        float skinWidth = 0.1f;

        //Debug.Log("normal: " + normal);
        //Debug.Log("tangent: " + tangent);

        // cast in +/- tangent
        Vector3 raycastOrigin = (Vector3) surfaceHit.point + normal * skinWidth;

        RaycastHit2D hit1 = Physics2D.Raycast(raycastOrigin, tangent, collider.size.x / 2 + skinWidth, crawler.crawlableLayers);
        RaycastHit2D hit2 = Physics2D.Raycast(raycastOrigin, -tangent, collider.size.x / 2 + skinWidth, crawler.crawlableLayers);

        //Debug.Log("hit1 collider: " + hit1.collider);
        //Debug.Log("hit2 collider: " + hit2.collider);
        float hit1Overshoot = hit1.collider != null ? Mathf.Max((collider.size.x / 2 + skinWidth) - hit1.distance, 0) : 0;
        float hit2Overshoot = hit2.collider != null ? Mathf.Max((collider.size.x / 2 + skinWidth) - hit2.distance, 0) : 0;

        //Debug.Log("Hit1 overshoot: " + hit1Overshoot);
        //Debug.Log("Hit2 overshoot: " + hit2Overshoot);
        Debug.Log("Collider size: " + collider.size);
        Vector3 verticalFudge = normal * crawler.transform.localScale.y * (collider.size.y / 2 + skinWidth - collider.offset.y);
        Vector3 horizontalFudge = tangent * hit2Overshoot - tangent * hit1Overshoot;

        Vector3 result = (Vector3)surfaceHit.point + verticalFudge + horizontalFudge;

        return result;
    }

    RaycastHit2D FindClosestSurfaceTo(Vector3 point)
    {
        Vector3[] raycastDirections = { Vector3.up, Vector3.down, Vector3.right, Vector3.left };

        RaycastHit2D closestSurface = raycastDirections
            .Select(dir => Physics2D.Raycast(point, dir, maxRaycastDistance, crawler.crawlableLayers))
            .Where(hit => hit.collider != null)
            .Aggregate((acc, next) => acc.distance < next.distance ? acc : next);

        originRotation = crawler.transform.rotation;
        destRotation = Quaternion.FromToRotation(Vector2.up, closestSurface.normal);

        //Debug.Log("normal: " + closestSurface.normal);
        //Debug.Log("up: " + crawler.transform.up);
        //Debug.Log("origin rotation: " + originRotation.eulerAngles);
        //Debug.Log("dest rotation: " + destRotation.eulerAngles);

        return closestSurface;
    }

    public void Exit()
    {
        crawler.transform.position = p2;
        crawler.transform.rotation = destRotation;

        crawler.animator.enabled = true;
    }

    public void HandleCollision(Collider2D collider)
    {
        // if player, fall to ground
        // if unexpected obstacle... cling? or fall?
    }

    // Update is called once per frame
    public void Update()
    {
        float normalizedTime = (Time.time - startTime) / jumpDuration;

        // If at the end of the arc, resume patrol state
        if (normalizedTime > 1)
        {
            crawler.sm.ChangeState(new Patrol(crawler));
            return;
        }

        // calculate position along arc, move to that point
        Vector3 pos = CalculateBezier(p0, p1, p2, normalizedTime);
        Quaternion rot = CalculateRotation(originRotation, destRotation, normalizedTime);

        crawler.transform.position = pos;
        crawler.transform.rotation = rot;
    }

    Vector3 CalculateBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        Vector3 result = (1 - t) * ((1 - t) * p0 + t * p1) + t * ((1 - t) * p1 + t * p2);

        return result;
    }

    Quaternion CalculateRotation(Quaternion origin, Quaternion dest, float t)
    {
        return Quaternion.Euler(0, 0, Mathf.LerpAngle(origin.eulerAngles.z, dest.eulerAngles.z, t * 3));
    }
}
