using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {
    public bool isFiring;
    public BulletController bullet;
    public float bulletSpeed;
    public float timeBetweenShots;
    private float shotCounter;
    public Transform firePoint;
    public int damage;
    public AudioSource gunSound;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(isFiring)
        {
            shotCounter -= Time.deltaTime;
            if(shotCounter <= 0)
            {
                shotCounter = timeBetweenShots;
                if (!gunSound.Equals(null))
                {
                    gunSound.Play();
                }
                BulletController newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation) as BulletController;
                newBullet.setDamage(damage);
                newBullet.speed = bulletSpeed;
            }
        } else
        {
            shotCounter = 0;
        }
		
	}

    public void SetSound(AudioSource sound)
    {
        gunSound = sound;
    }

    public void setDamage(int damage)
    {
        this.damage = damage;
    }
}
