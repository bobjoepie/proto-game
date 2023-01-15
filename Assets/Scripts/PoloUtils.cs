using System.Collections.Generic;
using UnityEngine;

public static class PoloUtils
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        return go.GetComponent<T>() ? go.GetComponent<T>() : go.AddComponent<T>();
    }

    public static T GetOrAddComponent<T>(this MonoBehaviour go) where T : Component
    {
        return go.GetComponent<T>() ? go.GetComponent<T>() : go.gameObject.AddComponent<T>();
    }

    public static Quaternion AngleTowards2D(this Vector2 position, Vector2 target)
    {
        var dir = position - target;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var rotToTarget = Quaternion.Euler(0f, 0f, angle);
        return rotToTarget;
    }

    public static Quaternion AngleTowards2D(this Vector3 position, Vector3 target)
    {
        var dir = (Vector2)position - (Vector2)target;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var rotToTarget = Quaternion.Euler(0f, 0f, angle);
        return rotToTarget;
    }

    public static Quaternion AngleTowards2D(this Vector2 position, Vector3 target)
    {
        var dir = (Vector2)position - (Vector2)target;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var rotToTarget = Quaternion.Euler(0f, 0f, angle);
        return rotToTarget;
    }

    public static Quaternion AngleTowards2D(this Vector3 position, Vector2 target)
    {
        var dir = (Vector2)position - (Vector2)target;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var rotToTarget = Quaternion.Euler(0f, 0f, angle);
        return rotToTarget;
    }

    public static int ToLayer(this LayerMask mask)
    {
        var layer = (int)Mathf.Log(mask.value, 2);
        return layer;
    }
}

