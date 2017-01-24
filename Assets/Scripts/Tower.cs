using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

	[Header ("Targeting")]
	[SerializeField]
	private float _targetRadius;
	[SerializeField]
	private float _rotationSpeed;
	[SerializeField]
	private LayerMask _layer;

	[Header ("Shooting")]
	[SerializeField]
	private float _damage;
	[SerializeField]
	private float _tickSpeed;
	[SerializeField]
	private float _minimalShootAngle;

	private GameObject _target;
	private Health _enemyHealth;
	private bool _hasTarget;
	private List<GameObject> _targetsInRange;
	private float _targetAngle;
	private float _angleWithTarget;
	private LineRenderer _lineRenderer;
	private float _nextTickTime;

    private void Awake() {
		_lineRenderer = GetComponent<LineRenderer> ();
	}

    private void Start() {
		_targetsInRange = new List<GameObject> ();
		_lineRenderer.enabled = false;
		_lineRenderer.SetPosition (0, transform.position);
	}

    private void Update() {
		Collider[] cols = Physics.OverlapSphere (transform.position, _targetRadius, _layer);
		_targetsInRange.Clear ();

		foreach (var col in cols) {
			_targetsInRange.Add (col.gameObject);
		}

		if (!_hasTarget) {
		    if (_targetsInRange.Count <= 0) return;
		    _target = _targetsInRange[0];
		    _enemyHealth = _target.GetComponent<Health>();
		    _hasTarget = true;
		    print("Has Target");
		} else {
			if (_targetsInRange.Contains (_target)) {
				RotateToTarget ();
				if (_angleWithTarget <= _minimalShootAngle) {
					_lineRenderer.SetPosition (1, _target.transform.position);
					_lineRenderer.enabled = true;
					TickDamage ();
				} else {
					_lineRenderer.enabled = false;
				}
			} else  {
				_target = null;
				_hasTarget = false;
				_lineRenderer.enabled = false;
				print ("Lost target");
			}
		}

	}

    private void RotateToTarget() {
		// the simple, fast way of rotating
		//transform.LookAt (target.transform.position);

		// another non-physics way of rotating, interpolating the rotation
		// we need to use the function above to calucate the desired angle
		Vector3 direction = _target.transform.position - transform.position;
		_targetAngle = Mathf.Atan2 (direction.x, direction.z) * Mathf.Rad2Deg;
		_angleWithTarget = Vector3.Angle (direction, transform.forward);

		transform.rotation = Quaternion.Lerp(transform.localRotation, 
								Quaternion.Euler(new Vector3(0f, _targetAngle, 0f)),
								_rotationSpeed * Time.deltaTime);
 	}

	private void TickDamage() {
		if (Time.time >= _nextTickTime) {
			_enemyHealth.TakeDamage (_damage);
			_nextTickTime = Time.time + _tickSpeed;
			print ("Shoot");
		}
	}

    private void OnDrawGizmos() {
		Gizmos.DrawWireSphere (transform.position, _targetRadius);
	}
}