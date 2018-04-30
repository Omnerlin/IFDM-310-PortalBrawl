using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    private int attStat;
    private int defStat;
    public int baseAtt;
    public int baseDef;
    public float timeForEffect;
    private float timerAttBuff;
    private float timerDefBuff;
    private Player myPlayer;

    // Use this for initialization
    void Start () {
        myPlayer = gameObject.GetComponent<Player>();
        attStat = baseAtt;
        defStat = baseDef;
        timerAttBuff = 0;
        timerDefBuff = 0;
		
	}
	
	// Update is called once per frame
	void Update () {
        if(attStat > baseAtt)
        {
            if(timerAttBuff >= timeForEffect)
            {
                attStat = baseAtt;
                timerAttBuff = 0;
            }
            else
            {
                timerAttBuff += Time.deltaTime;
            }

        }

        if (defStat > baseDef)
        {
            if (timerDefBuff >= timeForEffect)
            {
                defStat = baseDef;
                timerDefBuff = 0;
            }
            else
            {
                timerDefBuff += Time.deltaTime;
            }

        }

    }

    public void setAttStat(int stat)
    {
        attStat = stat;
    }

    public int getAttStat()
    {
        return attStat;
    }

    public void setDefStat(int stat)
    {
        defStat = stat;
    }

    public int getDefStat()
    {
        return defStat;
    }


}
