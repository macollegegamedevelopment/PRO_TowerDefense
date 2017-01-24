using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BeamTower : Tower {

    [Header ("Shooting")]
    [SerializeField]
    private float _damage;
    [SerializeField]
    private float _tickSpeed;
    [SerializeField]
    private float _minimalShootAngle;

    private GameObject _target;
    private Health _enemyHealth;

    private float _targetAngle;
    private float _angleWithTarget;
    private LineRenderer _lineRenderer;
    private float _nextTickTime;

    private void Awake() {
        _lineRenderer = GetComponent<LineRenderer> ();
    }

    protected override void Start() {
        base.Start();
        _lineRenderer.enabled = false;
        _lineRenderer.SetPosition (0, transform.position);
    }

    protected override void OnTargetEnter() {
        _target = TargetsInRange[0];
        _enemyHealth = _target.GetComponent<Health>();
    }

    protected override bool OnTargetStay()
    {
        if (TargetsInRange.Contains(_target))
        {
            RotateToTarget();
            if (_angleWithTarget <= _minimalShootAngle)
            {
                _lineRenderer.SetPosition(1, _target.transform.position);
                _lineRenderer.enabled = true;
                TickDamage();
            }
            return true;
        }
        return false;
    }

    protected override void OnTargetExit() {
        _target = null;
        _lineRenderer.enabled = false;
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
            RotationSpeed* Time.deltaTime);
    }

    private void TickDamage() {
        if (Time.time >= _nextTickTime) {
            _enemyHealth.TakeDamage (_damage);
            _nextTickTime = Time.time + _tickSpeed;
            print ("Shoot");
        }
    }
}

