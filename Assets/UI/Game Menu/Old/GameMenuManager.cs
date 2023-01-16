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
    
    private GM_MenuScreen PauseMenu;
    private GM_InventoryScreen InventoryScreen;
    public GM_ExpBar ExperienceBar { get; private set; }
    public GM_LevelDisplay LevelDisplay { get; private set; }
    public GM_SkillPrompt SkillPrompt { get; private set; }
    public GM_Selector Selector { get; private set; }

    public new class UxmlFactory : UxmlFactory<GameMenuManager, UxmlTraits> { }

    public GameMenuManager()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
        Instance = this;
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        DialogueView = this.Q("DialogueView");
        GameHudView = this.Q("game-hud-screen");

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
        
        PauseMenu = (GM_MenuScreen)this.Q("menu-screen");
        InventoryScreen = (GM_InventoryScreen)this.Q("inventory-screen");
        ExperienceBar = (GM_ExpBar)this.Q("exp-bar");
        LevelDisplay = (GM_LevelDisplay)this.Q("level-display");
        SkillPrompt = (GM_SkillPrompt)this.Q("skill-screen");
        Selector = this.Q<GM_Selector>();

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
        HealthBar.style.width = Length.Percent(100f * curHealth / maxHealth);
    }

    public void ClearInventoryList()
    {
        foreach (var item in InventoryList)
        {
            item.text = string.Empty;
        }
    }

    public void SetInventoryItem(InventoryItemOld[] inventory)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            InventoryItemOld itemOld = inventory[i];
            if (itemOld == null)
            {
                return;
            }
            var inventorySlot = InventoryList[i];
            inventorySlot.text = itemOld.name;
            inventorySlot.style.backgroundImage = new StyleBackground(itemOld.sprite);
            inventorySlot.style.unityBackgroundImageTintColor = itemOld.color;
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

    public void TogglePauseMenu()
    {
        PauseMenu.ToggleView();
    }

    public void ToggleInventoryWindow()
    {
        InventoryScreen.ToggleView();
    }
}
