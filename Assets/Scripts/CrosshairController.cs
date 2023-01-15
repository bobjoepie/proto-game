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
        Vector2 mousePos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        parentTransform.rotation = parentPos.AngleTowards2D(mousePos);
    }
}
