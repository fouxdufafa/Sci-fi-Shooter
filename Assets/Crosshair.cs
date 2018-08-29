using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Crosshair : MonoBehaviour, IInputControllable {

    float degreesRotation;
    [SerializeField] float maxDegreesRotation = 2f;

    public FixedUpdateCharacterInput InputController
    {
        get { return input; }
        set { input = value; }
    }
    private FixedUpdateCharacterInput input;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float angle = Mathf.Rad2Deg * Mathf.Atan2(input.Horizontal, input.Vertical);
        Quaternion desiredRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, maxDegreesRotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 5);
    }
}
