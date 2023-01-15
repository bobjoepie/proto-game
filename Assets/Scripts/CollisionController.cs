using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    public AppendageController appendageController;

    private void Awake()
    {
        appendageController = transform.parent.GetComponent<AppendageController>();
        gameObject.layer = appendageController.collisionLayer.ToLayer();
    }
}
