using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using UnityEngine;
using UnityEngine.UIElements;

public class UIDocManager : MonoBehaviour
{
    public static UIDocManager Instance { get; private set; }
    public UIDocument document;
    public GameMenuManager gameMenuManager;
    public GM_Modals Modals { get; private set; }
    public GM_ShopMenu ShopMenu { get; private set; }

    private void OnEnable()
    {
        Instance = this;
    }
    
    void Start()
    {
        gameMenuManager = document.rootVisualElement.Q<GameMenuManager>();
        Modals = (GM_Modals)gameMenuManager.Q("modals");
        ShopMenu = (GM_ShopMenu)gameMenuManager.Q("shop-screen");
    }
}
