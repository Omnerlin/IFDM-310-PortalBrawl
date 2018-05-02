using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Dialogue d;

    private Queue<string> sentencesQ;

    // Use this for initialization
    void Start()
    {
        sentencesQ = new Queue<string>();
        StartDialogue(d);
    }

    public void StartDialogue(Dialogue d)
    {
        //Debug.Log("Starting conversation with " + d.name);
        nameText.text = d.charName;

        sentencesQ.Clear();

        foreach (string s in d.sentences)
        {
            sentencesQ.Enqueue(s);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentencesQ.Count == 0)
        {
            EndDialogue();
            return;
        }

        string currentS = sentencesQ.Dequeue();
        //Debug.Log("trying to display next sentence!");

        //dialogueText.text = currentS;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentS));
    }

    IEnumerator TypeSentence(string currentS)
    {
        dialogueText.text = "";
        foreach (char c in currentS.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.07f);
        }
    }

    public void EndDialogue()
    {
        Debug.Log("The End!");
    }
}
