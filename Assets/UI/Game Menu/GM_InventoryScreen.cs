using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GM_InventoryScreen : VisualElement
{
    public new class UxmlFactory : UxmlFactory<GM_InventoryScreen, UxmlTraits> {}

    public GM_InventoryScreen()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    public void ToggleView()
    {
        this.ToggleInClassList("inventory-screen");
        this.ToggleInClassList("inventory-screen-active");
    }
}
