using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

	[SerializeField]
	private float targetRadius;
	[SerializeField]
	private LayerMask layer;

	private GameObject target;
	private bool hasTarget;
	private List<GameObject> targetsInRange;

	void Start() {
		targetsInRange = new List<GameObject> ();
	}

	void Update() {
		Collider[] cols = Physics.OverlapSphere (transform.position, targetRadius, layer);
		targetsInRange.Clear ();

		foreach (Collider col in cols) {
			targetsInRange.Add (col.gameObject);
		}

		if (!hasTarget) {
			if (targetsInRange.Count > 0) {
				target = targetsInRange [0];
				hasTarget = true;
				print ("Has Target");
			}
		} else {
			if (targetsInRange.Contains (target)) {
				print ("shoot");
			} else  {
				target = null;
				hasTarget = false;
				print ("lost target");
			}
		}

	}

	void OnDrawGizmos() {
		Gizmos.DrawWireSphere (transform.position, targetRadius);
	}
}