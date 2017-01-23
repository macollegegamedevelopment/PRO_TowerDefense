using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the path class is a simple container for waypoints.
// each enemy that spawns has to get the waypoints from this class
public class Path : MonoBehaviour {
	[SerializeField]
	private Transform startpoint;
	[SerializeField]
	private Transform[] waypoints;

	[SerializeField]
	bool drawPath;

	public Transform[] GetWaypoints() {
		return waypoints;
	}

	void OnDrawGizmos() {

		if (drawPath) {
			Gizmos.DrawLine (startpoint.position, waypoints [0].position);
			for (int i = 1; i < waypoints.Length; i++) {
				Gizmos.DrawLine (waypoints [i].position, waypoints [i - 1].position);
			}
		}
	}
}
