using UnityEngine;

public class Selectable : MonoBehaviour
{
    public string message;
    private Camera MainCamera;
    private bool IsHoverSelectorActive;
    private bool IsSelectorActive;
    private UIDocManagerOld docManagerOld;
    private GameStateManager gameStateManager;
    //public InteractableType modalType;
    //public string speechModalMessage;
    public float fadeTime;

    private void Start()
    {
        MainCamera = Camera.main;
        IsHoverSelectorActive = false;
        IsSelectorActive = false;

        docManagerOld = UIDocManagerOld.Instance;
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
        docManagerOld.Selector.MoveHoverSelector(screenPos);
    }

    private void MoveSelector()
    {
        Vector2 screenPos = MainCamera.WorldToViewportPoint(transform.root.position);
        docManagerOld.Selector.MoveSelector(screenPos);
    }

    private void OnMouseEnter()
    {
        docManagerOld.Selector.ToggleHoverSelector();
        IsHoverSelectorActive = true;
    }

    private void OnMouseExit()
    {
        docManagerOld.Selector.ToggleHoverSelector();
        IsHoverSelectorActive = false;
    }

    private void OnMouseUp()
    {
        gameStateManager.ClearSelectedObject();
        gameStateManager.SetSelectedObject(this);
        docManagerOld.Selector.ToggleSelector();
        gameStateManager.DontClearThisFrame();
        IsSelectorActive = true;
    }

    public void ClearSelection()
    {
        docManagerOld.Selector.ToggleSelector();
        IsSelectorActive = false;
    }
}
