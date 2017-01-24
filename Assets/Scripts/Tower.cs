using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour {

	[Header ("Targeting")]
	[SerializeField]
	protected float TargetRadius;
	[SerializeField]
	protected float RotationSpeed;
	[SerializeField]
	private LayerMask _layer;

	private  bool _hasTarget;
    protected List<GameObject> TargetsInRange;

    protected abstract void OnTargetEnter();
    protected abstract bool OnTargetStay();
    protected abstract void OnTargetExit();

    protected virtual void Start() {
		TargetsInRange = new List<GameObject> ();
	}

    protected virtual void Update() {
        var cols = Physics.OverlapSphere(transform.position, TargetRadius, _layer);
        TargetsInRange.Clear();

        foreach (var col in cols) {
            TargetsInRange.Add(col.gameObject);
        }

        if (!_hasTarget) {
            if (TargetsInRange.Count <= 0) return;
            OnTargetEnter();
            _hasTarget = true;
            print("Has Target");
        } else {
            if (!OnTargetStay()) {
                OnTargetExit();
                _hasTarget = false;
                print("Lost target");
            }
        }
    }

    private void OnDrawGizmos() {
		Gizmos.DrawWireSphere (transform.position, TargetRadius);
	}
}