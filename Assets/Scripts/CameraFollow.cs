using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public PlayerController player;

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
        var middlePos = Vector3.Lerp(mousePos, player.gameObject.transform.position, 0.75f);
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, middlePos, 5f * Time.deltaTime);
        
    }
}
