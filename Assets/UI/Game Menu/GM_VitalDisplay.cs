using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class GM_VitalDisplay : VisualElement
{
    private VisualElement HealthBar;
    private Label HealthLabel;
    public new class UxmlFactory : UxmlFactory<GM_VitalDisplay, UxmlTraits> { }

    public GM_VitalDisplay()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        HealthBar = this.Q("health-bar-foreground");
        HealthLabel = (Label)this.Q("health-bar-label");
        SetHealthBarAsync(100, 100).Forget();
        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    public void SetHealthBar(int curHealth, int maxHealth)
    {
        SetHealthBarAsync(curHealth, maxHealth).Forget();
    }

    private async UniTaskVoid SetHealthBarAsync(int curHealth, int maxHealth)
    {
        await UniTask.NextFrame();
        HealthBar.style.width = Length.Percent(100f * curHealth / maxHealth);
        HealthLabel.text = $"{curHealth} / {maxHealth}";
    }

    public void Enable()
    {

    }

    public void Disable()
    {

    }
}
