using UnityEngine;

public class EnemySpawner : MonoBehaviour 
{
	[SerializeField]
	private GameObject _objectToSpawn;

	private Vector3 _spawnPosition;
	private float _spawnDelay = 1f;

    private void Start() {
		_spawnPosition = new Vector3 (transform.position.x,
		                    _objectToSpawn.transform.position.y,
		                    transform.position.z);
	}

    private void Update() {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Spawn ();
		}
	}

    private void Spawn() {
        var e = Instantiate(_objectToSpawn, _spawnPosition, Quaternion.identity);
		e.transform.SetParent (transform);
	}
}