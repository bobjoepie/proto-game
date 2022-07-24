using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GM_MenuScreen : VisualElement
{
    public new class UxmlFactory : UxmlFactory<GM_MenuScreen, UxmlTraits> { }
    public GM_MenuScreen()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }
}
