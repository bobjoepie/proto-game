using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRotate : MonoBehaviour
{
    public Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = transform.position.AngleTowards2D(mousePos);
        var localScale = transform.localScale;
        transform.localScale = mousePos.x < transform.root.position.x ? new Vector3(localScale.x, -1, localScale.z) : new Vector3(localScale.x, 1, localScale.z);
    }
}
