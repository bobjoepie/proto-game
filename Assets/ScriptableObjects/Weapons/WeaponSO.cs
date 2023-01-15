using System;
using UnityEngine;

[Serializable]
public abstract class WeaponSO : PickupSO
{
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
                        wepInstance.gameObject.layer = (int)layer;
                        foreach (Transform child in wepInstance.transform)
                        {
                            child.gameObject.layer = (int)layer;
                        }
                    }

                    part.UpdatePartValues(wepInstance, iterationNum);
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
    public Weapon weaponGameObject;
    public int damage;
    [Range(-360, 360)] public float pre_direction;
    public SpawnLocation weaponSpawn;

    public void UpdatePartValues(Weapon wepInstance, int iterationNum)
    {
        switch (this)
        {
            case ProjectilePart proj:
                proj.UpdateValues(wepInstance as ProjectileController, iterationNum);
                break;
            case SpherePart sphere:
                sphere.UpdateValues(wepInstance as SphereController, iterationNum);
                break;
            case LinePart line:
                line.UpdateValues(wepInstance as LineController, iterationNum);
                break;
        }
    }
}
