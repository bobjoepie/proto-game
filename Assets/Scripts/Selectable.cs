using UnityEngine;

public class Selectable : MonoBehaviour
{
    public string message;
    private Camera MainCamera;
    private bool IsHoverSelectorActive;
    private bool IsSelectorActive;
    private UIDocManager docManager;
    private GameStateManager gameStateManager;
    //public InteractableType modalType;
    //public string speechModalMessage;
    public float fadeTime;

    private void Start()
    {
        MainCamera = Camera.main;
        IsHoverSelectorActive = false;
        IsSelectorActive = false;

        docManager = UIDocManager.Instance;
        gameStateManager = GameStateManager.Instance;
        if (transform != transform.root)
        {
            transform.root.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
        
    }

    private void Update()
    {
        if (IsHoverSelectorActive)
        {
            MoveHoverSelector();
        }

        if (IsSelectorActive)
        {
            MoveSelector();
        }
    }

    private void MoveHoverSelector()
    {
        Vector2 screenPos = MainCamera.WorldToViewportPoint(transform.root.position);
        docManager.Selector.MoveHoverSelector(screenPos);
    }

    private void MoveSelector()
    {
        Vector2 screenPos = MainCamera.WorldToViewportPoint(transform.root.position);
        docManager.Selector.MoveSelector(screenPos);
    }

    private void OnMouseEnter()
    {
        docManager.Selector.ToggleHoverSelector();
        IsHoverSelectorActive = true;
    }

    private void OnMouseExit()
    {
        docManager.Selector.ToggleHoverSelector();
        IsHoverSelectorActive = false;
    }

    private void OnMouseUp()
    {
        gameStateManager.ClearSelectedObject();
        gameStateManager.SetSelectedObject(this);
        docManager.Selector.ToggleSelector();
        gameStateManager.DontClearThisFrame();
        IsSelectorActive = true;
    }

    public void ClearSelection()
    {
        docManager.Selector.ToggleSelector();
        IsSelectorActive = false;
    }
}
