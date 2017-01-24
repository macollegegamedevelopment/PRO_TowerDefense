using UnityEngine;

public class Health : MonoBehaviour {

	float _currentHealth = 100;

	public void TakeDamage(float dmg) {
		_currentHealth -= dmg;

		if (_currentHealth <= 0) {
			print ("Enemy dies");
		}
	}
}
