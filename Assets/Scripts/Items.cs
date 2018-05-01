using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour {
    public enum ItemType
    {
        HEAL,
        ATT_UP,
        DEF_UP,
        MYSTERY
    }

    public ItemType item;
    public int ammountToHeal;
    public int attUp;
    public int defUp;
    private GameObject[] enimiesToDestroy;
    private GameObject musicManager;

    
	// Use this for initialization
	void Start () {
        musicManager = GameObject.FindGameObjectWithTag("Music");
		
	}
	
	// Update is called once per frame
	void Update () {
       
        
	}

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag.Equals("Player"))
    //    {
    //        if (item.Equals(ItemType.HEAL))
    //        {
    //            if (collision.gameObject.GetComponent<Player>().getMyData().currentHealth == collision.gameObject.GetComponent<Player>().maxHP)
    //            {
    //                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
    //                return;
    //            }
    //            collision.gameObject.GetComponent<Player>().healPlayer(ammountToHeal);
    //            Destroy(gameObject);
    //        }
    //        if (item.Equals(ItemType.ATT_UP))
    //        {
    //            if (collision.gameObject.GetComponent<PlayerStats>().getAttStat() > collision.gameObject.GetComponent<PlayerStats>().baseAtt)
    //            {
    //                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
    //                return;
    //            }
    //            collision.gameObject.GetComponent<PlayerStats>().setAttStat(attUp);
    //            Destroy(gameObject);
    //        }
    //        if (item.Equals(ItemType.DEF_UP))
    //        {
    //            if (collision.gameObject.GetComponent<PlayerStats>().getDefStat() > collision.gameObject.GetComponent<PlayerStats>().baseDef)
    //            {
    //                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
    //                return;
    //            }
    //            collision.gameObject.GetComponent<PlayerStats>().setDefStat(defUp);
    //            Destroy(gameObject);
    //        }
    //        if (item.Equals(ItemType.MYSTERY))
    //        {
                
    //            enimiesToDestroy = GameObject.FindGameObjectsWithTag("Enemy");
    //            foreach (GameObject i in enimiesToDestroy)
    //            {
    //                Destroy(i);
    //            }
    //            Destroy(gameObject);
    //        }
            
    //    }
    //    else if (collision.gameObject.tag.Equals("Enemy"))
    //    {
    //        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
    //        return;
    //    }
    //}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.root.tag.Equals("Player"))
        {
            if (item.Equals(ItemType.HEAL))
            {
                if (collision.gameObject.transform.root.GetComponent<Player>().getMyData().currentHealth == collision.gameObject.transform.root.GetComponent<Player>().maxHP)
                {
                    return;
                }
                collision.gameObject.transform.root.GetComponent<Player>().healPlayer(ammountToHeal);
                Destroy(gameObject);
            }
            if (item.Equals(ItemType.ATT_UP))
            {
                if (collision.gameObject.transform.root.GetComponent<PlayerStats>().getAttStat() > collision.gameObject.transform.root.GetComponent<PlayerStats>().baseAtt)
                {
                    return;
                }
                collision.gameObject.transform.root.GetComponent<PlayerStats>().setAttStat(attUp);
                Destroy(gameObject);
            }
            if (item.Equals(ItemType.DEF_UP))
            {
                if (collision.gameObject.transform.root.GetComponent<PlayerStats>().getDefStat() > collision.gameObject.transform.root.GetComponent<PlayerStats>().baseDef)
                {
                    return;
                }
                collision.gameObject.transform.root.GetComponent<PlayerStats>().setDefStat(defUp);
                Destroy(gameObject);
            }
            if (item.Equals(ItemType.MYSTERY))
            {

                enimiesToDestroy = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject i in enimiesToDestroy)
                {
                    Destroy(i);
                }
                Destroy(gameObject);
            }

        }
    }

    private void OnDestroy()
    {
        if(musicManager)
        musicManager.GetComponent<PickUpSoundPlayer>().PlaySound();
    }
}
