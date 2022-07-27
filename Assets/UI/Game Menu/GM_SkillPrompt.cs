using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GM_SkillPrompt : VisualElement
{
    private VisualElement skillPrompt;
    private List<Button> skillButtons;
    private int levelUpAmount;
    public new class UxmlFactory : UxmlFactory<GM_SkillPrompt, UxmlTraits> { }

    public GM_SkillPrompt()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        skillPrompt = this.Q("skill-prompt");
        skillButtons = skillPrompt
            .Children()
            .Where(e => e is Button)
            .Cast<Button>()
            .ToList();
        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
        levelUpAmount = 0;
    }

    public void AddToSkillQueue(PlayerController playerController)
    {
        if (levelUpAmount == 0)
        {
            ToggleSkillPrompt();
            InitializeButtons(playerController);
        }
        levelUpAmount += 1;
    }

    public void ToggleSkillPrompt()
    {
        skillPrompt.ToggleInClassList("skill-prompt");
        skillPrompt.ToggleInClassList("skill-prompt-active");
    }

    public void SkillButtonHandler(PlayerController player, int index)
    {
        levelUpAmount -= 1;
        switch (index)
        {
            case 0:
                player.maxHealth += 5;
                player.health += 5;
                GameMenuManager.Instance.SetHealthBar(player.health, player.maxHealth);
                break;
            case 1:
                player.speed += 1;
                break;
            case 2:
                if (player.cooldownRate > 0.15f)
                {
                    player.cooldownRate -= 0.10f;
                }
                break;
            default:
                break;
        }
        
        DisableButtons();
        ToggleSkillPrompt();
        if (levelUpAmount > 0)
        {
            ToggleSkillPrompt();
            InitializeButtons(player);
        }
    }

    private void InitializeButtons(PlayerController playerController)
    {
        for (var i = 0; i < skillButtons.Count; i++)
        {
            var x = i;
            var button = skillButtons[i];
            button.text = i switch
            {
                0 => "HP +",
                1 => "Move Speed +",
                2 => "Attack Speed +",
                _ => "Blank"
            };
            button.clickable = new Clickable(() => SkillButtonHandler(playerController, x));
        }
    }

    private void DisableButtons()
    {
        foreach (var button in skillButtons)
        {
            button.clickable = null;
        }
    }
}
