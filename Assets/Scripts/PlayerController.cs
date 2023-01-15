using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : EntityController
{
    private InputManager input;
    private AttackController attackController;
    private Camera mainCamera;
    public WeaponSO equippedWeapon;
    public ItemSO equippedItem;

    public bool CanUseItem;

    private GM_EquipmentDisplay equipmentDisplay;
    private GM_VitalDisplay vitalDisplay;

    private void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            input = InputManager.Instance;
        }
        input.Register(this, DefaultActionMaps.MovementKeyActions);
        input.Register(this, DefaultActionMaps.MenuKeyActions);
        input.Register(this, DefaultActionMaps.MouseKeyActions);
        input.Register(this, DefaultActionMaps.NumberKeyActions);
    }

    private void Start()
    {
        attackController = GetComponent<AttackController>();
        mainCamera = Camera.main;
        if (UIDocManager2.Instance != null)
        {
            equipmentDisplay = UIDocManager2.Instance.equipmentDisplay;
            vitalDisplay = UIDocManager2.Instance.vitalDisplay;
        }
        vitalDisplay.SetHealthBar(attributes.curHealth, attributes.maxHealth);
    }

    private void Update()
    {
        HandleMovement();
        HandleMenuing();
        HandleEquipment();
        HandleWeapons();
        HandleItems();
    }

    private void OnDisable()
    {
        input.Unregister(this);
    }

    private void HandleMovement()
    {
        var horizontal = 0f;
        var vertical = 0f;

        if (input.PollKey(this, KeyAction.Left))
        {
            horizontal -= 1f;
        }
        if (input.PollKey(this, KeyAction.Right))
        {
            horizontal += 1f;
        }
        if (input.PollKey(this, KeyAction.Up))
        {
            vertical += 1f;
        }
        if (input.PollKey(this, KeyAction.Down))
        {
            vertical -= 1f;
        }

        var movement = new Vector3(horizontal, vertical, 0).normalized;

        transform.position += new Vector3(
            movement.x * attributes.moveSpeed * Time.deltaTime, 
            movement.y * attributes.moveSpeed * Time.deltaTime, 
            0); 
    }

    private void HandleMenuing()
    {
        if (input.PollKeyDown(this, KeyAction.Tab))
        {
            //GameMenuManager.Instance.ToggleInventoryWindow();
        }
        else if (input.PollKeyDown(this, KeyAction.Escape))
        {
            //GameMenuManager.Instance.TogglePauseMenu();
        }
    }

    private void HandleEquipment()
    {
        for (int i = 0; i < DefaultActionMaps.NumberKeyActions.Count; i++)
        {
            if (input.PollKeyDown(this, DefaultActionMaps.NumberKeyActions[i]))
            {
                EquipFromInventory(i);
            }
        }
    }

    private void HandleWeapons()
    {
        if (input.PollKey(this, KeyAction.LeftClick) && equippedWeapon != null)
        {
            var targetPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            attackController.Attack(targetPos).Forget();
        }
    }

    private void HandleItems()
    {
        if (CanUseItem && input.PollKey(this, KeyAction.LeftClick) && equippedItem != null)
        {

        }
    }

    private void EquipFromInventory(int i)
    {
        equippedWeapon = null;
        equippedItem = null;
        equipmentDisplay.ClearDisplay();
        var inventoryItem = inventory.ElementAtOrDefault(i);
        if (inventoryItem == null) return;

        switch (inventoryItem.pickupObj)
        {
            case WeaponSO wp:
                equippedWeapon = wp;
                equipmentDisplay.SetPrimaryWeapon(wp);
                break;
            case ItemSO it:
                equippedItem = it;
                break;
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        vitalDisplay.SetHealthBar(attributes.curHealth, attributes.maxHealth);
    }
}