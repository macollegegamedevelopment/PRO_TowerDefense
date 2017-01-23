using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class EnemyMovement : MonoBehaviour {

	[SerializeField]
	private float movementSpeed;
	[SerializeField]
	[Range(0.1f, 3)]
	private float waypointActionRadius;

	private Rigidbody rigidbody;
	private Transform[] waypoints;
	private int waypointIndex;
	Vector3 targetPosition;
	bool reachedEndOfPath;

	void Awake() {
		rigidbody = GetComponent<Rigidbody> ();	
	}

	void Start() {
		Path levelPath = GameObject.FindGameObjectWithTag ("Path").GetComponent<Path>();
		waypoints = levelPath.GetWaypoints ();
		waypointIndex = 0;
		reachedEndOfPath = false;
		RotateToWaypoint ();
	}

	void Update() {
		if (!reachedEndOfPath) {
			float distance = Vector3.Distance (targetPosition, transform.position);
			if (distance <= waypointActionRadius) {
				if (waypointIndex < waypoints.Length-1) {
					waypointIndex++;
					RotateToWaypoint ();
				} else {
					reachedEndOfPath = true;
					print ("Reached End Of Path");
				}
			}
		}
	}

	void FixedUpdate() {
		if (!reachedEndOfPath) {
			Vector3 direction = transform.forward;
			Vector3 velocity = direction * movementSpeed * Time.fixedDeltaTime;
			rigidbody.MovePosition (rigidbody.position + velocity);
		}
	}

	void RotateToWaypoint() {
		// for now, we instantly set the rotation to a specific target.
		// later we could interpolate between 2 points, creating a smoother rotation
		targetPosition = new Vector3 (waypoints [waypointIndex].position.x,
			                   transform.position.y,
			                   waypoints [waypointIndex].position.z);
		
		transform.LookAt (targetPosition);

	}
}