using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{
    private InputManager input;

    private void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            input = InputManager.Instance;
        }
        input.Register(this, DefaultActionMaps.MovementKeyActions);
        input.Register(this, DefaultActionMaps.MenuKeyActions);
        input.Register(this, DefaultActionMaps.MouseKeyActions);
        input.Register(this, new List<KeyAction>() { KeyAction.SpaceKey });
    }

    private void Update()
    {
        HandleMovement();
        HandleMenuing();
        if (input.PollKeyDown(this, KeyAction.SpaceKey))
        {
            input.Unregister(this, DefaultActionMaps.MovementKeyActions);
        }
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
            GameMenuManager.Instance.ToggleInventoryWindow();
        }
        else if (input.PollKeyDown(this, KeyAction.Escape))
        {
            GameMenuManager.Instance.TogglePauseMenu();
        }
    }
}