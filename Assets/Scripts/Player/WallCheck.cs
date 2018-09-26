using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour {

    [SerializeField] float radius;
    [SerializeField] LayerMask whatIsWall;
    [SerializeField] Mode mode;
    Vector3 raycastDirection = Vector3.right;

    enum Mode
    {
        Raycast = 1,
        CircleCast = 2
    }

    public class WallContact
    {
        GameObject wall;
        GameObject hitWall;
        Vector2 point;
        
        public WallContact(GameObject wall, GameObject hitWall, Vector2 point)
        {
            this.wall = wall;
            this.hitWall = hitWall;
            this.point = point;
        }

        public GameObject Wall { get { return wall; } }
        public GameObject HitWall { get { return hitWall; } }
        public Vector2 ContactPoint { get { return point; } }
    }

    // Adjacent wall, if any
    public WallContact Contact { get; private set; }
	
	// Update is called once per frame
	void Update ()
    {
        RaycastHit2D[] hits;
        switch (mode)
        {
            case Mode.CircleCast:
                hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero, 0f, whatIsWall);
                break;
            case Mode.Raycast:
                raycastDirection = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;
                hits = Physics2D.RaycastAll(transform.position, raycastDirection, radius, whatIsWall);
                break;
            default:
                hits = new RaycastHit2D[0];
                break;
        }
        
        if (hits.Length > 0)
        {
            Contact = new WallContact(hits[0].collider.gameObject, gameObject, hits[0].point);
        }
        else
        {
            Contact = null;
        }
	}
}
