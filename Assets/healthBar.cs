using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBar : MonoBehaviour {

	public GameObject bar;

	//Sets the bar to the correct percentage of currentHealth to maxHealth.
	//could also display currentHealth in a text component on the bar.
	public void updateBar (int currentHealth, int maxHealth)
	{
		bar.transform.localScale = new Vector3 ((currentHealth / maxHealth), 1, 1);
	}
}
