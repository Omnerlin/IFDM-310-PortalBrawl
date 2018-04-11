using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour {

	public GameObject fullBar;

	//private HealthGradient gradient;

	Gradient g;
	GradientColorKey[] gck;
	GradientAlphaKey[] gak;


	void Start()
	{
		//gradient = new HealthGradient ();
		g = new Gradient ();
		gck = new GradientColorKey[3];
		gck [0].color = Color.red;
		gck [0].time = 0.0F;
		gck [1].color = Color.yellow;
		gck [1].time = 0.5F;
		gck [2].color = Color.green;
		gck [2].time = 1.0F;

		gak = new GradientAlphaKey[1];
		gak [0].alpha = 1.0f;
		gak [0].time = 0.0F;

		g.SetKeys (gck,gak);
	}

	//Sets the bar to the correct percentage of currentHealth to maxHealth.
	//could also display currentHealth in a text component on the bar.
	public void updateBar (int currentStat, int maxStat)
	{
		updateBar((float)currentStat,(float)maxStat);
	}

	public void updateBar (float currentStat, float maxStat)
	{
        if(maxStat <= 0)
        {
            Debug.LogError("maxStat on statbar is less than or equal to zero. you probably don't want this");
            return;
        }

		float barPercentFull = currentStat / maxStat;
		fullBar.transform.localScale = new Vector3 (barPercentFull, 1, 1);
		fullBar.GetComponent<Image>().color = g.Evaluate(barPercentFull);

	}
		
}
