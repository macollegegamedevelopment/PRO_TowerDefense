using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [HideInInspector] public GameObject Target;

    [SerializeField] private float _missileSpeed;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // ugly fix for now, should do this with steering behaviour
    private void Update()
    {
        transform.LookAt(Target.transform);
    }

    private void FixedUpdate()
    {
        var direction = transform.forward;
        var velocity = direction * _missileSpeed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(_rigidbody.position + velocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) Destroy(this.gameObject);
    }
}
