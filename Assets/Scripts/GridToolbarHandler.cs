using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GridToolbarHandler : MonoBehaviour
{
    public Color hoverColor;
    public Color selectColor;
    public List<GridToolbarButton> toolbarButtons;
    public GameObject currentDragElement;
    public Camera mainCamera;
    public GridHandler gridHandler;

    private Sequence curSequence;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        foreach (var button in GetComponentsInChildren<GridToolbarButton>())
        {
            button.gameObject.AddComponent<BoxCollider2D>();
            button.hoverColor = hoverColor;
            button.selectColor = selectColor;
            button.toolbarHandler = this;
            button.gridHandler = gridHandler;
            switch (button)
            {
                case GridToolbarTrashButton trashButton:
                    toolbarButtons.Add(trashButton);
                    break;
                case GridToolbarSubmitButton submitButton:
                    toolbarButtons.Add(submitButton);
                    break;
                case GridToolbarCreateButton createButton:
                    toolbarButtons.Add(createButton);
                    break;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDragElement != null)
        {
            curSequence.Kill();
            curSequence = DOTween.Sequence();
            var worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            curSequence.Append(currentDragElement.transform.DOMove(new Vector3(worldPos.x, worldPos.y), 0.1f));
            
            curSequence.Play();
        }
    }

    public void SetCurrentDragObject(GameObject element)
    {
        currentDragElement = element;
    }

    public void ClearCurrentDragObject(bool kill = false)
    {
        curSequence.Kill();
        curSequence = null;
        if (kill)
        {
            Destroy(currentDragElement);
        }
        currentDragElement = null;
    }
}
