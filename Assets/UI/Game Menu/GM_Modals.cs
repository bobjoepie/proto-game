using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GM_Modals : VisualElement
{
    private VisualElement interactModal;
    private Label interactModalLabel;
    private VisualElement interactModalLabelOrigin;
    public new class UxmlFactory : UxmlFactory<GM_Modals, UxmlTraits> { }

    public GM_Modals()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        interactModal = this.Q("modal-interact-box");
        interactModalLabel = (Label)interactModal.Q("modal-label");
        interactModalLabelOrigin = this.Q("modal-interact-box-origin");
        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    public void ToggleModal()
    {
        interactModal.ToggleInClassList("modal-interact-box");
        interactModal.ToggleInClassList("modal-interact-box-active");
    }

    public void ChangeText(string text)
    {
        interactModalLabel.text = text;
    }

    public void MoveModal(Vector2 position)
    {
        interactModalLabelOrigin.style.left = Length.Percent(position.x * 100f);
        interactModalLabelOrigin.style.top = StyleKeyword.Auto;
        interactModalLabelOrigin.style.right = StyleKeyword.Auto;
        interactModalLabelOrigin.style.bottom = Length.Percent(position.y * 100f);
    }
}
