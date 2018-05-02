using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPortal : MonoBehaviour {

    public enum PortalState { Spawning, Exiting }
    private PortalState currentState = PortalState.Spawning;

    // The collider used for deactivating players.
    public Collider2D innerCollider;

    // The collider used for pulling in players
    public Collider2D pullEffectorCollider;

    // Keep track of players that have been sucked by the portal
    private List<GameObject> suckedPlayers = new List<GameObject>();

    private void Awake()
    {
        innerCollider.enabled = false;
        pullEffectorCollider.enabled = false;
    }

    // Use this for initialization
    void Start () {
	}

    public void DoTheThing()
    {
        if(currentState == PortalState.Spawning)
        {
            Debug.Log("BOI, we're loading up this thing");
            PlayerManager.instance.LoadPlayers();
        }
        else
        {
            BeginSuckingPlayers();
        }
    }

    private void BeginSuckingPlayers()
    {
        innerCollider.enabled = true;
        pullEffectorCollider.enabled = true;

        // Check for any dead players, and add them to the list since they can't move
        foreach (GameObject playa in PlayerManager.instance.activePlayers)
        {
            if(playa.GetComponent<Player>().isDead())
            {
                suckedPlayers.Add(playa);
            }
            else
            {
                Collider2D[] colliders = playa.GetComponentsInChildren<Collider2D>();
                foreach  (Collider2D col in colliders)
                {
                    col.isTrigger = true;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if(currentState == PortalState.Exiting && collision.transform.root.tag == "Player" && !suckedPlayers.Contains(collision.gameObject) && 
            innerCollider.IsTouching(collision) && !collision.transform.root.gameObject.GetComponent<Player>().isDead())
        {
            // You done sucked up a playa
            suckedPlayers.Add(collision.gameObject);
            collision.transform.root.gameObject.SetActive(false);

            if(suckedPlayers.Count == PlayerManager.instance.activePlayers.Count)
            {
                // It is time to d-d-d-d-d-d-d-dd-d-d-- go to the next level. ._.
                GetComponent<Animator>().SetTrigger("Close");
            }
        }
    }

    public void PortalClose()
    {
        if(currentState == PortalState.Spawning)
        {
            this.gameObject.SetActive(false);
            currentState = PortalState.Exiting;
        }
        else
        {
            SceneTransitionManager.Instance.TransitionToScene(SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1),
                SceneTransitionManager.AnimationType.forward);
        }
    }

}
