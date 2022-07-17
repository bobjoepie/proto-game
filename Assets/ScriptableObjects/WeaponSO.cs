using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class WeaponSO : ScriptableObject
{
    public string name;
    [TextArea(3, 5)]
    public string description;
    [Range(0, 1)]
    public float cooldown;
    [Range(1,10)] public int amount;
    [Range(0, 1)] public float amountBurstTime;

    private void Reset()
    {
        amount = 1;
    }

    public static dynamic ConvertWeaponToParts(dynamic weaponSO)
    {
        dynamic weaponParts = Array.Empty<WeaponPart>();
        switch (weaponSO)
        {
            case ProjectileWepSO projObj:
                weaponParts = projObj.weaponParts;
                break;
            case SphereWepSO sphereObj:
                weaponParts = sphereObj.weaponParts;
                break;
            case LineWepSO lineObj:
                weaponParts = lineObj.weaponParts;
                break;
        }
        return weaponParts;
    }

    public static void InstantiateWeaponParts(WeaponPart[] weaponParts, Vector3 position, Quaternion rotation)
    {
        foreach (WeaponPart part in weaponParts)
        {
            switch (part.weaponSpawn)
            {
                case SpawnLocation.ClosestEnemy:
                    break;
                case SpawnLocation.ClosestMouse:
                    break;
                case SpawnLocation.Single:
                    break;
                default:
                    var wepInstance = Instantiate(part.weaponGameObject);
                    wepInstance.transform.position = position;
                    wepInstance.transform.rotation = rotation * Quaternion.Euler(0f, 0f, part.pre_direction);

                    part.UpdateValues(wepInstance);
                    break;
            }
        }
    }
}

public enum WeaponType
{
    Projectile,
    Single, //TODO
    Sphere,
    Cone, //TODO
    Line, //TODO
}

public enum TargetType
{
    None,
    TowardsMouse,
    Self, //TODO
    TowardsNearestEnemy, //TODO
    TowardsNearestMouse, //TODO
    TowardsInitialMouse,
    TowardsInitialEnemy, //TODO
}

public enum SpawnLocation
{
    None,
    Self,
    MousePoint,
    ClosestEnemy, //TODO
    ClosestMouse, //TODO
    Single, //TODO
    CustomPosition, //TODO
}

public enum CollisionType //TODO
{
    Enemy,
    All,
    Player,
    None
}

[Serializable]
public class WeaponPart
{
    public string label;

    [Header("Properties")]
    public WeaponType weaponType;
    public CollisionType collisionType;
    public GameObject weaponGameObject;
    public int damage;
    [Range(-360, 360)] public float pre_direction;
    public SpawnLocation weaponSpawn;

    public void UpdateValues(GameObject wepInstance)
    {
        switch (this)
        {
            case ProjectilePart proj:
                proj.UpdateValues(wepInstance.GetComponentInChildren<ProjectileController>());
                break;
            case SpherePart sphere:
                sphere.UpdateValues(wepInstance.GetComponentInChildren<SphereController>());
                break;
            case LinePart line:
                line.UpdateValues(wepInstance.GetComponentInChildren<LineController>());
                break;
        }
    }
}
