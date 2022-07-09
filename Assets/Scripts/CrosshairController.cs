using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public Transform parentTransform;

    private Camera MainCamera;
    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 parentPos = parentTransform.position;
        Vector2 mousePos = (Vector2)MainCamera.ScreenToWorldPoint(Input.mousePosition);
        var dir = parentPos - mousePos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        parentTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }
}
