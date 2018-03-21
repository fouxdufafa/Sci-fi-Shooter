using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour {

    [SerializeField] float radius;
    [SerializeField] public GameObject wallCheck;
    [SerializeField] LayerMask whatIsWall;
    Vector3 raycastDirection = Vector3.right;

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
    private WallContact contact = null;
    public WallContact Contact { get { return contact; }}

	// Use this for initialization
	void Start () {
        wallCheck = transform.Find("WallCheck").gameObject;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        raycastDirection = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;
        RaycastHit2D[] hits = Physics2D.RaycastAll(wallCheck.transform.position, raycastDirection, radius, whatIsWall);
        if (hits.Length > 0)
        {
            print(hits.Length);
            contact = new WallContact(hits[0].collider.gameObject, wallCheck, hits[0].point);
        }
        else
        {
            contact = null;
        }
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(wallCheck.transform.position, wallCheck.transform.position + raycastDirection * radius);
    }
}
