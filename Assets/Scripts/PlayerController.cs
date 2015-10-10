using UnityEngine;
using System.Collections;
using System;
[RequireComponent (typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    Rigidbody myRigidbody;
    Vector3 velocity;
	// Use this for initialization
	void Start () {
        myRigidbody = GetComponent<Rigidbody>();
	}


    internal void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }
    internal void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    protected void FixedUpdate()
    {
        myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);
    }
}



