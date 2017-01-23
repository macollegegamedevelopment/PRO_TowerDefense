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
	private float angleToTarget;
	private LineRenderer lineRenderer;
	private float nextShootTime;

	void Awake() {
		lineRenderer = GetComponent<LineRenderer> ();
	}

	void Start() {
		targetsInRange = new List<GameObject> ();
		lineRenderer.enabled = false;
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
				if ((Vector3.Angle(target.transform.position, transform.forward)) < minimalShootAngle) {
					lineRenderer.SetPosition (1, target.transform.position);
					lineRenderer.enabled = true;
					TickDamage ();
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
		angleToTarget = Mathf.Atan2 (direction.x, direction.z) * Mathf.Rad2Deg;
	}

	void RotateToTarget() {
		// the simple, fast way of rotating
		//transform.LookAt (target.transform.position);

		// another non-physics way of rotating, interpolating the rotation
		// we need to use the function above to calucate the desired angle
		transform.rotation = Quaternion.Lerp(transform.localRotation, 
			Quaternion.Euler(new Vector3(0f, angleToTarget, 0f)), 
								rotationSpeed * Time.deltaTime);
 	}

	void TickDamage() {
		if (Time.time >= nextShootTime) {
			enemyHealth.TakeDamage (damage);
			nextShootTime = Time.time + tickSpeed;
			print ("Shoot");
		}
	}
	void OnDrawGizmos() {
		Gizmos.DrawWireSphere (transform.position, targetRadius);
	}
}