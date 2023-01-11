using UnityEngine;

public class GridToolbarCreateButton : GridToolbarButton
{
    private bool isDragged;
    private DragElement element;
    public GameObject dragObj;

    public override void Awake()
    {
        base.Awake();
        element = new DragElement(); 
        element.image = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        element.dragObj = transform.GetChild(0).gameObject;
        isDragged = false;
    }

    private void OnMouseDown()
    {
        if (isDragged) return;
        isDragged = true;
        TransitionColor(selectColor, 0.25f);
        dragObj = Instantiate(element.dragObj);
        var worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        dragObj.transform.position = new Vector3(worldPos.x, worldPos.y, 0);
        dragObj.transform.rotation = Quaternion.identity;
        dragObj.layer = LayerMask.NameToLayer("Ignore Raycast");

        var spriteRenderer = dragObj.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = element.image;
        spriteRenderer.color = Color.white;
        toolbarHandler.SetCurrentDragObject(dragObj);
    }

    private void OnMouseUp()
    {
        if (!isDragged) return;
        var worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var hit = Physics2D.Raycast(worldPos, Vector2.zero);
        var hitObj = hit.collider;
        if (hitObj != null && hitObj.TryGetComponent<GridCell>(out var gridCell))
        {
            gridCell.childObj = dragObj;
            dragObj.transform.parent = hitObj.gameObject.transform;
            dragObj.transform.localPosition = Vector2.zero;
            TransitionColor(origColor, 0.25f);
            isDragged = false;
            toolbarHandler.ClearCurrentDragObject();
        }
        else if (isDragged)
        {
            TransitionColor(origColor, 0.25f);
            isDragged = false;
            toolbarHandler.ClearCurrentDragObject(true);
        }
    }

    public override void OnMouseOver()
    {
        if (!isDragged)
        {
            base.OnMouseOver();
        }
    }

    public override void OnMouseExit()
    {
        if (!isDragged)
        {
            base.OnMouseExit();
        }
    }
}

public class DragElement
{
    public string name;
    public string description;
    public int value;
    public Sprite image;
    public GameObject dragObj;
}
