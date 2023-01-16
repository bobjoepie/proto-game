using UnityEngine;
using UnityEngine.UIElements;

public class GM_Selector : VisualElement
{
    private VisualElement hoverSelector;
    private VisualElement hoverSelectorOrigin;

    private VisualElement selector;
    private VisualElement selectorOrigin;
    public new class UxmlFactory : UxmlFactory<GM_Selector, UxmlTraits> { }

    public GM_Selector()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        hoverSelector = this.Q("hover-selector");
        hoverSelectorOrigin =  this.Q("hover-selector-origin");

        selector = this.Q("selector");
        selectorOrigin = this.Q("selector-origin");
        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    public void ToggleHoverSelector()
    {
        hoverSelector.ToggleInClassList("hover-selector");
        hoverSelector.ToggleInClassList("hover-selector-active");
    }

    public void MoveHoverSelector(Vector2 position)
    {
        hoverSelectorOrigin.style.left = Length.Percent(position.x * 100f);
        hoverSelectorOrigin.style.top = StyleKeyword.Auto;
        hoverSelectorOrigin.style.right = StyleKeyword.Auto;
        hoverSelectorOrigin.style.bottom = Length.Percent(position.y * 100f);
    }

    public void ToggleSelector()
    {
        selector.ToggleInClassList("selector");
        selector.ToggleInClassList("selector-active");
    }

    public void MoveSelector(Vector2 position)
    {
        selectorOrigin.style.left = Length.Percent(position.x * 100f);
        selectorOrigin.style.top = StyleKeyword.Auto;
        selectorOrigin.style.right = StyleKeyword.Auto;
        selectorOrigin.style.bottom = Length.Percent(position.y * 100f);
    }
}
