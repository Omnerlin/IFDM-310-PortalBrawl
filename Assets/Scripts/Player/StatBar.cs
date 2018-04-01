using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBar : MonoBehaviour {

	public GameObject fullBar;

	//Sets the bar to the correct percentage of currentHealth to maxHealth.
	//could also display currentHealth in a text component on the bar.
	public void updateBar (int currentStat, int maxStat)
	{
		fullBar.transform.localScale = new Vector3 ((currentStat / maxStat), 1, 1);
	}

	public void updateBar (float currentStat, float maxStat)
	{
		fullBar.transform.localScale = new Vector3 ((currentStat / maxStat), 1, 1);
	}
}
