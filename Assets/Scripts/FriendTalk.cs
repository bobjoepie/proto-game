using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendTalk : MonoBehaviour
{
    [Range(5f, 10f)]
    public float textSpeed;
    public string[] dialogue;
    public string[] dialogue2;
    public AudioManager audioManager;
    public AudioClip audioClip;
    
    private int dialogueLine;
    private bool isTalking;
    private bool sayNext;
    private bool canProceed;
    private bool alreadyTalked;
    private GameObject player;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        isTalking = false;
        dialogueLine = 0;
        sayNext = true;
        canProceed = false;
        alreadyTalked = false;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTalking && sayNext)
        {
            SayDialogue(dialogueLine);
        }
        else if (isTalking && canProceed && Input.GetKeyDown(KeyCode.Space))
        {
            sayNext = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTalking)
        {
            player = collision.gameObject;
            isTalking = true;
            player.GetComponent<PlayerController>().speed = 0;
            GameMenuManager.Instance.ShowDialogue();
        }
    }

    private void SayDialogue(int line)
    {
        int length = alreadyTalked ? dialogue2.Length : dialogue.Length;
        if (line == length)
        {
            EndDialogue();
        }
        else
        {
            canProceed = false;
            sayNext = false;
            string lineText = alreadyTalked ? dialogue2[line] : dialogue[line];
            StartCoroutine(ReadText(lineText));
        }
    }

    IEnumerator ReadText(string text)
    {
        GameMenuManager.Instance.ClearDialogue();
        GameMenuManager.Instance.HideContinuePrompt();
        foreach (char c in text.ToCharArray())
        {
            GameMenuManager.Instance.AddToDialogue(c);
            if (char.IsLetterOrDigit(c))
            {
                audioSource.PlayOneShot(audioClip);
            }
            yield return new WaitForSeconds(0.5f/textSpeed);
        }
        GameMenuManager.Instance.ShowContinuePrompt();
        canProceed = true;
        dialogueLine += 1;
    }

    private void EndDialogue()
    {
        player.GetComponent<PlayerController>().speed = 4;
        player = null;
        dialogueLine = 0;
        sayNext = true;
        isTalking = false;
        alreadyTalked = true;
        GameMenuManager.Instance.HideDialogue();
    }

}
