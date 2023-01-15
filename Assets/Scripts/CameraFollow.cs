using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var offset = mousePos - target.gameObject.transform.position;
        var clamp = Vector3.ClampMagnitude(offset, 3f);
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, target.gameObject.transform.position + clamp, 5f * Time.deltaTime);
        
    }
}
