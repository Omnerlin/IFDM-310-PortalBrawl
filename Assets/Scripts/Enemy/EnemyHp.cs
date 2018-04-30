using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour {

    public int hP;
    public int currentHP;
    public Items[] itemsToDrop;

    [Tooltip("Material to switch to while enemy is hurt")]
    public Material flashMaterial;
    public float flashTime = 0.1f;

    public float chanceToDropMystery;
    

    private Material baseMat; // make sure we keep a reference to what base material the enemy uses

    private void Awake()
    {
        baseMat = GetComponent<SpriteRenderer>().material;
    }
    // Use this for initialization
    void Start () {
        currentHP = hP;
        GetComponent<SpriteRenderer>().material.color = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void HurtEnemy(int damage)
    {
        currentHP -= damage;
        Debug.Log("Enemy hurt for " + damage + ": health now at " + currentHP);

        if (currentHP <= 0)
        {
            float randNum = Random.Range(0.0f,1.0f);
            // Debug.Log(randNum + "number");
            if(randNum < chanceToDropMystery)
            {
                Instantiate(itemsToDrop[3], transform.position, transform.rotation);
                Destroy(gameObject);
            }
            else
            {
                Instantiate(itemsToDrop[Random.Range(0, itemsToDrop.Length - 1)], transform.position, transform.rotation);
                Destroy(gameObject);
            }
            
            
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(flashHurtMat());
        }
    }

    IEnumerator flashHurtMat()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        float timer = flashTime;
        renderer.material = flashMaterial;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            // renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, (timer / flashTime));
            yield return null;
        }

        renderer.material = baseMat;
    }
}
