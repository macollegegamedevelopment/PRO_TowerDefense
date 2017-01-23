using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

	float currentHealth = 100;

	public void TakeDamage(float dmg) {
		currentHealth -= dmg;

		if (currentHealth <= 0) {
			print ("Enemy dies");
		}
	}
}
