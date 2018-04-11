using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This class is simply meant to hold references to the UI display components.
//This way, a pointer to this PlayerStatDisplay can be passed to each Player object,
//and that Player object can then reference whatever stat/health bars are here.
public class PlayerStatDisplay : MonoBehaviour {

    public Text text;

	public GameObject healthBar;
	public GameObject ultimateBar;

	public Text getText () {return (Text) text;}
	public GameObject getHealthBar () {return healthBar;}
	public GameObject getUltimateBar () {return ultimateBar;}
}
