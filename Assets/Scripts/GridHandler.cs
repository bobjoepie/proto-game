using System.Collections.Generic;
using UnityEngine;

public class GridHandler : MonoBehaviour
{
    public int gridX;
    public int gridY;
    public float margin;
    public Color hoverColor;
    public Color selectColor;
    public List<GridCell> grid;
    public GridToolbarHandler gridToolbarHandler;
    public GameObject gridGameObject;

    void Start()
    {
        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridY; j++)
            {
                var cell = Instantiate(gridGameObject, this.transform, true);
                cell.transform.localPosition = new Vector3(j * margin, i * margin, 0);
                cell.gameObject.AddComponent<BoxCollider2D>();
                var gridCell = cell.gameObject.AddComponent<GridCell>();
                gridCell.gridHandler = this;
                gridCell.hoverColor = hoverColor;
                gridCell.selectColor = selectColor;
                
                grid.Add(gridCell);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
