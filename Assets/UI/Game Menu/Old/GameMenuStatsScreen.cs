using UnityEngine.UIElements;

public class GameMenuStatsScreen : VisualElement
{
    public new class UxmlFactory : UxmlFactory<GameMenuStatsScreen, UxmlTraits> { }
    public GameMenuStatsScreen()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }
}
