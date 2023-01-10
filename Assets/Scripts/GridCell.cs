using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    private Color origColor;
    public Color hoverColor;
    public Color selectColor;
    private SpriteRenderer colorObj;
    private bool isDragged;
    public GridHandler gridHandler;
    public GameObject childObj;

    private void Awake()
    {
        colorObj = GetComponent<SpriteRenderer>();
        origColor = colorObj.color;
        isDragged = false;
    }

    private void OnMouseDown()
    {
        if (isDragged || childObj == null) return;
        isDragged = true;
        childObj.transform.SetParent(null, true);
        var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        childObj.transform.position = new Vector3(worldPos.x, worldPos.y, 0);
        childObj.transform.rotation = Quaternion.identity;
        childObj.layer = LayerMask.NameToLayer("Ignore Raycast");

        gridHandler.gridToolbarHandler.SetCurrentDragObject(childObj);
    }

    private void OnMouseUp()
    {
        if (!isDragged) return;
        var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var hit = Physics2D.Raycast(worldPos, Vector2.zero);
        var hitObj = hit.collider;
        if (hitObj != null)
        {
            if (hitObj.TryGetComponent<GridCell>(out var gridCell))
            {
                switch (gridCell.transform.childCount)
                {
                    case 0:
                        gridCell.childObj = childObj;
                        childObj.transform.parent = hitObj.gameObject.transform;
                        childObj.transform.localPosition = Vector2.zero;
                        if (!gridCell.isDragged)
                        {
                            childObj = null;
                        }
                        isDragged = false;
                        gridHandler.gridToolbarHandler.ClearCurrentDragObject();
                        return;
                    case 1:
                        var swapCellObj = gridCell.childObj;
                        gridCell.childObj = childObj;
                        gridCell.childObj.transform.parent = hitObj.gameObject.transform;
                        gridCell.childObj.transform.localPosition = Vector2.zero;

                        swapCellObj.transform.parent = gameObject.transform;
                        swapCellObj.transform.localPosition = Vector2.zero;
                        childObj = swapCellObj;

                        isDragged = false;
                        gridHandler.gridToolbarHandler.ClearCurrentDragObject();
                        return;
                }
            }
            else if (hitObj.TryGetComponent<GridToolbarButton>(out var gridToolbarButton))
            {
                switch (gridToolbarButton)
                {
                    case GridToolbarTrashButton trashButton:
                        isDragged = false;
                        gridHandler.gridToolbarHandler.ClearCurrentDragObject(true);
                        return;
                }
            }
        }
        childObj.transform.parent = gameObject.transform;
        childObj.transform.localPosition = Vector2.zero;
        isDragged = false;
        gridHandler.gridToolbarHandler.ClearCurrentDragObject();
    }

    private void OnMouseOver()
    {
        if (gridHandler.gridToolbarHandler.currentDragElement != null || transform.childCount > 0)
        {
            TransitionColor(hoverColor, 0.15f);
        }
        else
        {
            TransitionColor(origColor, 0.15f);
        }
    }

    private void OnMouseExit()
    {
        TransitionColor(origColor, 0.15f);
    }

    private void TransitionColor(Color color, float duration = 0.25f)
    {
        DOTween.To(
            () => colorObj.color, 
            x => colorObj.color = x, 
            color, 
            duration
            );
    }
}
