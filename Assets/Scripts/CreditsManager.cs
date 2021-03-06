﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsManager : MonoBehaviour {
    public GameObject[] credits;
    public GameObject target1;
    public GameObject target2;
    public float fadeTime = 4.0f;

    private Queue<GameObject> credQ;
    private bool scrolling = false;
    private bool scrollingAgain = false;

	// Use this for initialization
	void Start () {
        credQ = new Queue<GameObject>();
        RollCredits();
	}

    public void RollCredits() {
        credQ.Clear();

        foreach (GameObject t in credits) {
            credQ.Enqueue(t);
        }

        DisplayNext();
    }

    public void DisplayNext() {
        if (credQ.Count == 0)
        {
            EndCredits();
            return;
        }

        GameObject currentText = credQ.Dequeue();
        StopAllCoroutines();
        StartCoroutine(ScrollCredit(currentText));
    }

    private void Update()
    {
        // Let's make it so that people don't have to watch our awesome names if they don't want to.
        foreach (Rewired.Player player in Rewired.ReInput.players.GetPlayers())
        {
            if (player.GetButtonDown("Start") || player.GetButtonDown("XButton"))
            {
                EndCredits();
            }
        }
    }

    IEnumerator ScrollCredit(GameObject currentText) {

        Vector3 myPos = currentText.transform.position;
        Vector3 yourPos = target1.transform.position;
        Vector3 herPos = target2.transform.position;

        scrolling = true;
        while (scrolling) {
            while (Vector3.Distance(myPos, yourPos) > 5f) {
                currentText.transform.position = Vector3.SmoothDamp(myPos, yourPos, ref myPos, Time.deltaTime);
                myPos = currentText.transform.position;
                yield return new WaitForSeconds(0.2f);
            }
            scrolling = false;
        }

        yield return new WaitForSeconds(3);

        scrollingAgain = true;
        while (scrollingAgain) {
            while (Vector3.Distance(myPos, herPos) > 5f) {
                currentText.transform.position = Vector3.SmoothDamp(myPos, herPos, ref myPos, Time.deltaTime);
                myPos = currentText.transform.position;
                yield return new WaitForSeconds(0.2f);
            }
            scrollingAgain = false;
        }
        DisplayNext();
    }

    public void EndCredits() {
        Debug.Log("The End!");
        StopAllCoroutines();
        LoadScene ls = GetComponent<LoadScene>();
        ls.loadScene();
    }
}
