using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	
[RequireComponent (typeof(Rigidbody))]
public class EnemyMovement : MonoBehaviour {

	[SerializeField]
	private float movementSpeed;

	private Rigidbody rigidbody;

	void Awake() {
		rigidbody = GetComponent<Rigidbody> ();	
	}

	void FixedUpdate() {
		Vector3 direction = Vector3.forward;
		Vector3 velocity = direction * movementSpeed * Time.fixedDeltaTime;
		rigidbody.MovePosition (rigidbody.position + velocity);
	}
}