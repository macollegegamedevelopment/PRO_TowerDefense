using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

	[Header ("Targeting")]
	[SerializeField]
	private float targetRadius;
	[SerializeField]
	private float rotationSpeed;
	[SerializeField]
	private LayerMask layer;

	[Header ("Shooting")]
	[SerializeField]
	private float damage;
	[SerializeField]
	private float tickSpeed;
	[SerializeField]
	private float minimalShootAngle;

	private GameObject target;
	private Health enemyHealth;
	private bool hasTarget;
	private List<GameObject> targetsInRange;
	private float targetAngle;
	private float angleWithTarget;
	private LineRenderer lineRenderer;
	private float nextTickTime;

	void Awake() {
		lineRenderer = GetComponent<LineRenderer> ();
	}

	void Start() {
		targetsInRange = new List<GameObject> ();
		lineRenderer.enabled = false;
		lineRenderer.SetPosition (0, transform.position);
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
				enemyHealth = target.GetComponent<Health> ();
				hasTarget = true;
				print ("Has Target");
			}
		} else {
			if (targetsInRange.Contains (target)) {
				CalculateAngleToTarget ();
				RotateToTarget ();
				if (angleWithTarget <= minimalShootAngle) {
					lineRenderer.SetPosition (1, target.transform.position);
					lineRenderer.enabled = true;
					TickDamage ();
				} else {
					lineRenderer.enabled = false;
				}
			} else  {
				target = null;
				hasTarget = false;
				lineRenderer.enabled = false;
				print ("Lost target");
			}
		}

	}

	void CalculateAngleToTarget() {
		Vector3 direction = target.transform.position - transform.position;
		targetAngle = Mathf.Atan2 (direction.x, direction.z) * Mathf.Rad2Deg;
		angleWithTarget = Vector3.Angle (direction, transform.forward);
	}

	void RotateToTarget() {
		// the simple, fast way of rotating
		//transform.LookAt (target.transform.position);

		// another non-physics way of rotating, interpolating the rotation
		// we need to use the function above to calucate the desired angle
		transform.rotation = Quaternion.Lerp(transform.localRotation, 
								Quaternion.Euler(new Vector3(0f, targetAngle, 0f)), 
								rotationSpeed * Time.deltaTime);
 	}

	void TickDamage() {
		if (Time.time >= nextTickTime) {
			enemyHealth.TakeDamage (damage);
			nextTickTime = Time.time + tickSpeed;
			print ("Shoot");
		}
	}
	void OnDrawGizmos() {
		Gizmos.DrawWireSphere (transform.position, targetRadius);
	}
}