using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlip : MonoBehaviour
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
        var localScale = transform.localScale;
        transform.localScale = mousePos.x < transform.root.position.x ? new Vector3(-1,localScale.y, localScale.z) : new Vector3(1, localScale.y, localScale.z);
    }
}
