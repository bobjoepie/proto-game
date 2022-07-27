using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GM_ExpBar : VisualElement
{
    private VisualElement expBar;
    public new class UxmlFactory : UxmlFactory<GM_ExpBar, UxmlTraits> {}

    public GM_ExpBar()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }
    
    void OnGeometryChange(GeometryChangedEvent evt)
    {
        expBar = this.Q("exp-bar-foreground");
        expBar.style.width = Length.Percent(0f);
        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    public void ToggleView()
    {
    }

    public void SetEXPBar(int curExp, int maxExp)
    {
        expBar.style.width = Length.Percent(100f * curExp / maxExp);
    }
}
