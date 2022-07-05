using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitch : MonoBehaviour
{
    private Camera camera;
    public Vector3 position;
    private void Start()
    {
        camera = Camera.main;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            camera.transform.position = position;
        }
    }
}
