using System.Collections.Generic;
using System.Linq;
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

    private VisualElement InventoryBar;
    private List<Label> InventoryList;

    private VisualElement HealthBar;
    private Label HealthBarText;
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

        HealthBar = this.Q("health-bar-foreground");
        HealthBarText = (Label)this.Q("health-bar-label");
        SetHealthBar(100, 100);

        InventoryBar = this.Q("game-toolbar-top");
        InventoryList = InventoryBar
            .Children()
            .Where(e => e.name == "game-hud-slot")
            .Select(x => x
                .Children()
                .Where(c => c is Label)
                .Cast<Label>()
                .FirstOrDefault()
            )
            .ToList();
        ClearInventoryList();

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

    public void SetHealthBar(int curHealth, int maxHealth)
    {
        HealthBarText.text = curHealth + "/" + maxHealth;
        HealthBar.style.width = Length.Percent(100f * (float)curHealth / (float)maxHealth);
    }

    public void ClearInventoryList()
    {
        foreach (var item in InventoryList)
        {
            item.text = string.Empty;
        }
    }

    public void SetInventoryItem(List<InventoryItem> inventory)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            InventoryItem item = inventory[i];
            var inventorySlot = InventoryList[i];
            inventorySlot.text = item.name;
            inventorySlot.style.backgroundImage = new StyleBackground(item.sprite);
            inventorySlot.style.unityBackgroundImageTintColor = item.color;
        }
    }

    public void SelectItem(int index)
    {
        InventoryList[index].style.borderBottomColor = Color.red;
        InventoryList[index].style.borderLeftColor = Color.red;
        InventoryList[index].style.borderRightColor = Color.red;
        InventoryList[index].style.borderTopColor = Color.red;
        InventoryList[index].style.borderBottomWidth = new StyleFloat(2f);
        InventoryList[index].style.borderLeftWidth = new StyleFloat(2f);
        InventoryList[index].style.borderRightWidth = new StyleFloat(2f);
        InventoryList[index].style.borderTopWidth = new StyleFloat(2f);

    }

    public void ClearSelectedItem()
    {
        foreach (var slot in InventoryList)
        {
            slot.style.borderBottomWidth = new StyleFloat(0f);
            slot.style.borderLeftWidth = new StyleFloat(0f);
            slot.style.borderRightWidth = new StyleFloat(0f);
            slot.style.borderTopWidth = new StyleFloat(0f);
        }
    }
}
