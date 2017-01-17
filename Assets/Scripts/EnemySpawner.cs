using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour 
{
	[SerializeField]
	private GameObject objectToSpawn;

	private Vector3 spawnPosition;
	private float spawnDelay = 1f;

	void Start() {	
		spawnPosition = new Vector3 (transform.position.x, 0.25f, transform.position.z);
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Spawn ();
		}
	}

	void Spawn() {
		GameObject e = Instantiate (objectToSpawn, spawnPosition, Quaternion.identity) as GameObject;
		e.transform.SetParent (transform);
	}
}