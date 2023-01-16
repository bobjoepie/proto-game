using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class FriendTalk : MonoBehaviour
{
    [Range(5f, 30f)]
    public float textSpeed;
    public string[] dialogue;
    public string[] dialogue2;
    [FormerlySerializedAs("audioManager")] public AudioManagerOld audioManagerOld;
    public AudioClip audioClip;
    
    private int DialogueLine;
    private bool IsTalking;
    private bool SayNext;
    private bool CanProceed;
    private bool AlreadyTalked;
    private GameObject Player;
    private AudioSource AudioSource;
    // Start is called before the first frame update
    void Start()
    {
        IsTalking = false;
        DialogueLine = 0;
        SayNext = true;
        CanProceed = false;
        AlreadyTalked = false;
        AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsTalking) return;
        if (SayNext)
        {
            SayDialogue(DialogueLine);
        }
        else if (CanProceed && Input.GetKeyDown(KeyCode.Space))
        {
            SayNext = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || IsTalking) return;
        Player = collision.gameObject;
        IsTalking = true;
        Player.GetComponent<PlayerControllerOld>().speed = 0;
        GameMenuManager.Instance.ShowDialogue();
    }

    private void SayDialogue(int line)
    {
        int length = AlreadyTalked ? dialogue2.Length : dialogue.Length;
        if (line < length)
        {
            CanProceed = false;
            SayNext = false;
            string lineText = AlreadyTalked ? dialogue2[line] : dialogue[line];
            StartCoroutine(ReadText(lineText));
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator ReadText(string text)
    {
        GameMenuManager.Instance.ClearDialogue();
        GameMenuManager.Instance.HideContinuePrompt();
        foreach (char c in text)
        {
            GameMenuManager.Instance.AddToDialogue(c);
            if (char.IsLetterOrDigit(c))
            {
                AudioSource.PlayOneShot(audioClip);
            }
            yield return new WaitForSeconds(0.5f/textSpeed);
        }
        GameMenuManager.Instance.ShowContinuePrompt();
        CanProceed = true;
        DialogueLine += 1;
    }

    private void EndDialogue()
    {
        Player.GetComponent<PlayerControllerOld>().speed = 4;
        Player = null;
        DialogueLine = 0;
        SayNext = true;
        IsTalking = false;
        AlreadyTalked = true;
        transform.position += new Vector3(0, 15f, 0);
        GameMenuManager.Instance.HideDialogue();
    }

}
