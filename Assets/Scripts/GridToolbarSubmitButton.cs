using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridToolbarSubmitButton : GridToolbarButton
{
    private void OnMouseUpAsButton()
    {
        Debug.Log("Submit " + gridHandler.grid.Count(x => x.childObj != null));
    }
}
