using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobAnimation : MonoBehaviour
{
    public float speed = 5f;
    public float height = 0.5f;

    void Update()
    {
        Vector3 pos = transform.localPosition;
        float newY = Mathf.Sin(Time.time * speed);
        transform.localPosition = new Vector3(pos.x, newY, pos.z) * height;
    }
}
