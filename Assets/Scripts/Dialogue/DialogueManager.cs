using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Dialogue d1;
    public Dialogue d2;
    public Image backgroundImg;
    public Image blackPanel;
    public float fadeTime = 4.0f;
    public Sprite img1;
    public Sprite img2;
    public Button continueButton;

    private Dialogue currentD;
    private Queue<string> sentencesQ;

    // Use this for initialization
    void Start()
    {
        currentD = d1;
        sentencesQ = new Queue<string>();
        StartDialogue(d1);
    }

    private void Update()
    {
        IList<Rewired.Player> playas = Rewired.ReInput.players.GetPlayers();
        foreach (Rewired.Player p in playas)
        {
            if (p.GetButtonDown("XButton"))
            {
                continueButton.onClick.Invoke();
            }
        }
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

    IEnumerator MyFade()
    {
        //Debug.Log("Calling fade!");
        blackPanel.CrossFadeAlpha(255.0f, fadeTime, true);
        yield return new WaitForSeconds(5.0f);
        backgroundImg.sprite = img2;
        blackPanel.CrossFadeAlpha(0.0f, fadeTime, true);
        StartDialogue(d2);
    }


    public void EndDialogue()
    {
        if (currentD == d1)
        {
            currentD = d2;
            //StopAllCoroutines();
            StartCoroutine(MyFade());
        }
        //Debug.Log("The End!");
    }
}
