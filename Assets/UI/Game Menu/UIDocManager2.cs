using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIDocManager2 : MonoBehaviour
{
    public static UIDocManager2 Instance { get; private set; }
    public UIDocument document;
    public VisualElement root;
    public GM_BossHealthBar bossHealthBar;
    public GM_EquipmentDisplay equipmentDisplay;
    public GM_VitalDisplay vitalDisplay;

    private UIDocManager2()
    {
        Instance = this;
    }

    private void Awake()
    {
        root = document.rootVisualElement;
        bossHealthBar = root.Q<GM_BossHealthBar>();
        equipmentDisplay = root.Q<GM_EquipmentDisplay>();
        vitalDisplay = root.Q<GM_VitalDisplay>();
    }
}
