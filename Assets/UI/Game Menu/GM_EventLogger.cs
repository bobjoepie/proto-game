using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GM_EventLogger : VisualElement
{
    public new class UxmlFactory : UxmlFactory<GM_EventLogger, UxmlTraits> { }

    public GM_EventLogger()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }
}
