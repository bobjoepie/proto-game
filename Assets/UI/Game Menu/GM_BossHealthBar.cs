using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GM_BossHealthBar : VisualElement
{
    public new class UxmlFactory : UxmlFactory<GM_BossHealthBar, UxmlTraits> { }

    public GM_BossHealthBar()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }
}
