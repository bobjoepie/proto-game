using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum KeyAction
{
    LeftClick,
    RightClick,

    Up,
    Left,
    Down,
    Right,

    Use,
    SpaceKey,

    Slot1,
    Slot2,
    Slot3,
    Slot4,
    Slot5,
    Slot6,
    Slot7,
    Slot8,
    Slot9,
    Slot0,

    Tab,
    Escape,
}

public static class DefaultActionMaps
{
    public static List<KeyAction> MouseKeyActions = new List<KeyAction>()
    {
        KeyAction.LeftClick,
        KeyAction.RightClick,
    };

    public static List<KeyAction> MovementKeyActions = new List<KeyAction>()
    {
        KeyAction.Up,
        KeyAction.Left,
        KeyAction.Down,
        KeyAction.Right,
    };

    public static List<KeyAction> NumberKeyActions = new List<KeyAction>()
    {
        KeyAction.Slot1,
        KeyAction.Slot2,
        KeyAction.Slot3,
        KeyAction.Slot4,
        KeyAction.Slot5,
        KeyAction.Slot6,
        KeyAction.Slot7,
        KeyAction.Slot8,
        KeyAction.Slot9,
        KeyAction.Slot0,
    };

    public static List<KeyAction> MenuKeyActions = new List<KeyAction>()
    {
        KeyAction.Tab,
        KeyAction.Escape,
    };
}

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public Dictionary<EntityController, HashSet<KeyAction>> actionMaps = new Dictionary<EntityController, HashSet<KeyAction>>();

    private readonly Dictionary<KeyAction, KeyCode> KeyActionMap = new Dictionary<KeyAction, KeyCode>()
    {
        {KeyAction.LeftClick    ,       KeyCode.Mouse0},
        {KeyAction.RightClick   ,       KeyCode.Mouse1},

        {KeyAction.Up           ,       KeyCode.W},
        {KeyAction.Left         ,       KeyCode.A},
        {KeyAction.Down         ,       KeyCode.S},
        {KeyAction.Right        ,       KeyCode.D},

        {KeyAction.Use          ,       KeyCode.E},
        {KeyAction.SpaceKey     ,       KeyCode.Space},

        {KeyAction.Slot1        ,       KeyCode.Alpha1},
        {KeyAction.Slot2        ,       KeyCode.Alpha2},
        {KeyAction.Slot3        ,       KeyCode.Alpha3},
        {KeyAction.Slot4        ,       KeyCode.Alpha4},
        {KeyAction.Slot5        ,       KeyCode.Alpha5},
        {KeyAction.Slot6        ,       KeyCode.Alpha6},
        {KeyAction.Slot7        ,       KeyCode.Alpha7},
        {KeyAction.Slot8        ,       KeyCode.Alpha8},
        {KeyAction.Slot9        ,       KeyCode.Alpha9},
        {KeyAction.Slot0        ,       KeyCode.Alpha0},

        {KeyAction.Tab          ,       KeyCode.Tab},
        {KeyAction.Escape       ,       KeyCode.Escape},
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public bool PollKeyDown(EntityController entity, KeyAction action)
    {
        if (Input.GetKeyDown(KeyActionMap[action]) && actionMaps.ContainsKey(entity) && actionMaps[entity].Contains(action))
        {
            return true;
        }
        return false;
    }

    public bool PollKeyUp(EntityController entity, KeyAction action)
    {
        if (Input.GetKeyUp(KeyActionMap[action]) && actionMaps.ContainsKey(entity) && actionMaps[entity].Contains(action))
        {
            return true;
        }
        return false;
    }

    public bool PollKey(EntityController entity, KeyAction action)
    {
        if (Input.GetKey(KeyActionMap[action]) && actionMaps.ContainsKey(entity) && actionMaps[entity].Contains(action))
        {
            return true;
        }
        return false;
    }

    public void Register<T>(T entity, List<KeyAction> actionMap)
    {
        switch (entity)
        {
            case EntityController e:
                if (actionMaps.ContainsKey(e))
                {
                    actionMaps[e].UnionWith(actionMap);
                }
                else
                {
                    actionMaps.Add(e, actionMap.ToHashSet());
                }
                break;
        }
    }

    public void Unregister<T>(T entity, List<KeyAction> actionMap = null)
    {
        switch (entity)
        {
            case EntityController e:
                if (actionMap == null)
                {
                    actionMaps.Remove(e);
                }
                else if (actionMaps.ContainsKey(e))
                {
                    actionMaps[e].ExceptWith(actionMap);
                }
                break;
        }
    }
}
