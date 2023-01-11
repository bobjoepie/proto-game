using UnityEngine.UIElements;

public class GM_LevelDisplay : VisualElement
{
    private Label levelDisplay;
    public new class UxmlFactory : UxmlFactory<GM_LevelDisplay, UxmlTraits> { }

    public GM_LevelDisplay()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        levelDisplay = (Label)this.Q("level-label");
        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    public void SetLevel(int level)
    {
        levelDisplay.text = level.ToString();
    }
}
