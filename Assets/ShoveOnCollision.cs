using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoveOnCollision : MonoBehaviour {

    [SerializeField] GameObject target;
    [SerializeField] float shoveForce = 10f;

    private Vector3 lastContactNormal;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        System.Console.WriteLine(collision.gameObject);
        if (collision.gameObject.Equals(target))
        {
            ContactPoint2D[] contacts = new ContactPoint2D[10];
            print(collision.GetContacts(contacts));
            print("Got contact point");
            Vector2 normal = contacts[0].normal * -1;
            lastContactNormal = normal;
            Vector2 impulseDirection = normal.x >= 0 ? Vector2.right : Vector2.left;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(impulseDirection * shoveForce, ForceMode2D.Impulse);
        }
    }
}
