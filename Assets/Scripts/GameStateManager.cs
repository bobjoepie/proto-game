using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public Selectable selectedObject;
    private bool selectionClearable;

    private void OnEnable()
    {
        Instance = this;
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        //Debug.Log(selectedObject?.name);
        if (selectionClearable && Input.GetMouseButtonUp(0) && selectedObject != null)
        {
            ClearSelectedObject();
        }

        selectionClearable = true;
    }

    public void SetSelectedObject(Selectable obj)
    {
        selectedObject = obj;
    }

    public void ClearSelectedObject()
    {
        if (selectedObject != null)
        {
            selectedObject.ClearSelection();
            selectedObject = null;
        }
    }

    public void DontClearThisFrame()
    {
        selectionClearable = false;
    }
}
