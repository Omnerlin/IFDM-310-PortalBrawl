using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGradient {

	public HealthGradient()
	{
		Gradient g;
		GradientColorKey[] gck;
		GradientAlphaKey[] gak;
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
}