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

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            root = document.rootVisualElement;
        }
    }

}
