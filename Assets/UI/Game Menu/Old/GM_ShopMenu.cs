using UnityEngine.UIElements;

public class GM_ShopMenu : VisualElement
{
    private VisualElement shopWindow;
    public new class UxmlFactory : UxmlFactory<GM_ShopMenu, UxmlTraits> { }

    public GM_ShopMenu()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        shopWindow = this.Q("shop-window");
        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    public void ToggleShopWindow()
    {
        shopWindow.ToggleInClassList("shop-window");
        shopWindow.ToggleInClassList("shop-window-active");
    }

    public void DisableShopWindow()
    {
        shopWindow.ClearClassList();
        shopWindow.AddToClassList("shop-window");
    }
}
