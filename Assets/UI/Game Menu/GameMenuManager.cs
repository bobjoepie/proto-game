using UnityEngine;
using UnityEngine.UIElements;

public class GameMenuManager : VisualElement
{
    public static GameMenuManager Instance { get; private set; }
    VisualElement DialogueView;
    VisualElement dialogueBox;
    Label dialogueText;
    Label continuePrompt;
    public new class UxmlFactory : UxmlFactory<GameMenuManager, UxmlTraits> { }

    public GameMenuManager()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
        Instance = this;
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        DialogueView = this.Q("DialogueView");
        
        dialogueText = (Label)this.Q("dialogue-text");
        dialogueBox = (VisualElement)this.Q("dialogue-box");
        dialogueBox.visible = false;
        continuePrompt = (Label)this.Q("continue-prompt");
        continuePrompt.visible = false;

        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    public void ShowDialogue()
    {
        this.Q("DialogueView").visible = true;
        dialogueBox.visible = true;
        dialogueText.text = string.Empty;
    }

    public void HideDialogue()
    {
        this.Q("DialogueView").visible = false;
        dialogueBox.visible = false;
        continuePrompt.visible = false;
        dialogueText.text = string.Empty;
    }

    public void ClearDialogue()
    {
        dialogueText.text = string.Empty;
    }

    public void AddToDialogue(char c)
    {
        //dialogueText = (Label)this.Q("dialogue-text");
        dialogueText.text += c;
    }

    public void ShowContinuePrompt()
    {
        continuePrompt.visible = true;
    }

    public void HideContinuePrompt()
    {
        continuePrompt.visible = false;
    }
}
