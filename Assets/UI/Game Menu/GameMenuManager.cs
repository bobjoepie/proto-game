using UnityEngine;
using UnityEngine.UIElements;

public class GameMenuManager : VisualElement
{
    public static GameMenuManager Instance { get; private set; }
    private VisualElement DialogueView;
    private VisualElement GameHudView;
    private VisualElement DialogueBox;
    private Label DialogueText;
    private Label ContinuePrompt;
    public new class UxmlFactory : UxmlFactory<GameMenuManager, UxmlTraits> { }

    public GameMenuManager()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
        Instance = this;
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        DialogueView = this.Q("DialogueView");
        GameHudView = this.Q("GameHUDView");

        DialogueText = (Label)this.Q("dialogue-text");
        DialogueBox = this.Q("dialogue-box");
        DialogueBox.visible = false;
        ContinuePrompt = (Label)this.Q("continue-prompt");
        ContinuePrompt.visible = false;

        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    public void ShowDialogue()
    {
        this.Q("DialogueView").visible = true;

        DialogueView.style.display = DisplayStyle.Flex;
        GameHudView.style.display = DisplayStyle.None;

        DialogueBox.visible = true;
        DialogueText.text = string.Empty;
    }

    public void HideDialogue()
    {
        this.Q("DialogueView").visible = false;

        GameHudView.style.display = DisplayStyle.Flex;
        DialogueView.style.display = DisplayStyle.None;

        DialogueBox.visible = false;
        ContinuePrompt.visible = false;
        DialogueText.text = string.Empty;
    }

    public void ClearDialogue()
    {
        DialogueText.text = string.Empty;
    }

    public void AddToDialogue(char c)
    {
        //dialogueText = (Label)this.Q("dialogue-text");
        DialogueText.text += c;
    }

    public void ShowContinuePrompt()
    {
        ContinuePrompt.visible = true;
    }

    public void HideContinuePrompt()
    {
        ContinuePrompt.visible = false;
    }

    public void ShowGameHudView()
    {
        GameHudView.style.display = DisplayStyle.Flex;
        DialogueView.style.display = DisplayStyle.None;
    }
}
