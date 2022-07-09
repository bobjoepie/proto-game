using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitch : MonoBehaviour
{
    private Camera MainCamera;
    public Vector3 position;
    private void Start()
    {
        MainCamera = Camera.main;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MainCamera.transform.position = position;
        }
    }
}
