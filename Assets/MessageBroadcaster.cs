using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBroadcaster : MonoBehaviour {

    // Make this a singleton instance that broadcasts stuff
    public static MessageBroadcaster Instance;
    
    public float messageDuration = 2.5f; // In seconds. The amount of time a message will last if there aren't any others in the queue
    public float maxMessageDuration = 1.5f; // In seconds. The max amount of time a message will hang while there's more than one thing in the queue.
    public Text broadCastText; // Text that will be changed on message
    public RectTransform broadCastPanel; // Parent panel that will be animated

    private List<string> messageQueue = new List<string>(); // Queue of strings that will get broadcasted on the canvas

    private Animator panelAnimator; // The animator attached to the panel we want to use

    private bool announcing = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        // We don't want any weird behavior here.
        if(maxMessageDuration > messageDuration)
        {
            messageDuration = maxMessageDuration;
        }

        panelAnimator = broadCastPanel.GetComponent<Animator>();
    }

    // "Broadcast Message" was taken...
    // Anyways, this'll add a message to the queue and trigger the coroutine if necessary
    public void BroadcastAnnouncement(string message)
    {
        messageQueue.Add(message);

        if(!announcing)
        {
            StopAllCoroutines();
            StartCoroutine(AnnounceStuff());
        }
    }

    // This will run in the background while there's stuff in the queue
    // It'll stop doing stuff after there's no more messages
    private IEnumerator AnnounceStuff()
    {
        announcing = true;
        while(messageQueue.Count > 0)
        {
            // Send out the message and play the animation. Remove the message from the queue
            broadCastText.text = messageQueue[0];
            panelAnimator.SetTrigger("Open");
            messageQueue.Remove(messageQueue[0]);

            // If this message is the last one in the queue, make it last as long as the messageDuration
            yield return new WaitForSeconds(messageQueue.Count == 0 ? messageDuration : maxMessageDuration );
        }

        announcing = false;
        panelAnimator.SetTrigger("Close");
    }
}
