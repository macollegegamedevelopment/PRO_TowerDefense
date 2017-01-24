using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class EnemyMovement : MonoBehaviour {

	[SerializeField]
	private float _movementSpeed;
	[SerializeField]
	[Range(0.1f, 3)]
	private float _waypointActionRadius;

	private Rigidbody _rigidbody;
	private Transform[] _waypoints;
	private int _waypointIndex;
	private Vector3 _targetPosition;
	private  bool _reachedEndOfPath;

	private void Awake() {
		_rigidbody = GetComponent<Rigidbody> ();
	}

	private void Start() {
		Path levelPath = GameObject.FindGameObjectWithTag ("Path").GetComponent<Path>();
		_waypoints = levelPath.GetWaypoints ();
		_waypointIndex = 0;
		_reachedEndOfPath = false;
		RotateToWaypoint ();
	}

	private void Update() {
		if (!_reachedEndOfPath) {
			var distance = Vector3.Distance (_targetPosition, transform.position);
			if (distance <= _waypointActionRadius) {
				if (_waypointIndex < _waypoints.Length-1) {
					_waypointIndex++;
					RotateToWaypoint ();
				} else {
					_reachedEndOfPath = true;
					print ("Reached End Of Path");
				}
			}
		}
	}

	private void FixedUpdate() {
	    if (!_reachedEndOfPath)
	    {
	        var direction = transform.forward;
	        var velocity = direction * _movementSpeed * Time.fixedDeltaTime;
	        _rigidbody.MovePosition(_rigidbody.position + velocity);
	    }
	}

    private void RotateToWaypoint() {
		// for now, we instantly set the rotation to a specific target.
		// later we could interpolate between 2 points, creating a smoother rotation
		_targetPosition = new Vector3 (_waypoints [_waypointIndex].position.x,
			                   transform.position.y,
			                   _waypoints [_waypointIndex].position.z);
		
		transform.LookAt (_targetPosition);

	}
}