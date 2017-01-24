using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MissileTower : Tower
{
    [Header("Shooting")]
    [SerializeField] private Missile _missile;
    [SerializeField] private int _missilesPerBurst;
    [SerializeField] private float _missileBurstRate;
    [SerializeField] private float _shootRate;
    [SerializeField] private Transform _muzzle;

    private float _nextShootTime = 0;
    private bool _isShooting = false;

    protected override void Update()
    {
        base.Update();
        transform.Rotate(Vector3.up * RotationSpeed * Time.deltaTime);
    }

    protected override void OnTargetEnter() {}

    protected override bool OnTargetStay()
   {
        if (TargetsInRange.Count <= 0) return false;
        if (!_isShooting)
        {
            if (Time.time >= _nextShootTime)
            {
                StartCoroutine("Shoot");
            }
        }
        return true;
    }

    protected override void OnTargetExit() {}

    private IEnumerator Shoot()
    {
        var missilesShot = 0;
        var targets = TargetsInRange.ToArray();

        print(targets[0]);
        _isShooting = true;
        while (missilesShot < _missilesPerBurst)
        {
            SpawnMissile(targets[missilesShot%targets.Length]);
            missilesShot++;
            yield return new WaitForSeconds(_missileBurstRate);
        }
        _nextShootTime = Time.time + _shootRate;
        _isShooting = false;
    }

    private void SpawnMissile(GameObject missileTarget)
    {
        var missile = Instantiate(_missile, _muzzle.position, _muzzle.rotation);
        missile.Target = missileTarget;
        missile.transform.SetParent(transform);
    }
}
