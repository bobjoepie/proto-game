using System;
using UnityEngine;

[Serializable]
public abstract class WeaponSO : PickupSO
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

    public static void InstantiateWeaponParts(WeaponPart[] weaponParts, Vector3 position, Quaternion rotation, int? layer, int iterationNum = 0)
    {
        if (iterationNum >= 5)
        {
            return;
        }
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
                    if (layer != null)
                    {
                        wepInstance.layer = (int)layer;
                        var children = wepInstance.GetComponentsInChildren<Transform>(includeInactive: true);
                        foreach (var child in children)
                        {
                            child.gameObject.layer = (int)layer;
                        }
                    }

                    part.UpdateValues(wepInstance, iterationNum);
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

    public void UpdateValues(GameObject wepInstance, int iterationNum)
    {
        switch (this)
        {
            case ProjectilePart proj:
                proj.UpdateValues(wepInstance.GetComponentInChildren<ProjectileController>(), iterationNum);
                break;
            case SpherePart sphere:
                sphere.UpdateValues(wepInstance.GetComponentInChildren<SphereController>(), iterationNum);
                break;
            case LinePart line:
                line.UpdateValues(wepInstance.GetComponentInChildren<LineController>(), iterationNum);
                break;
        }
    }
}
