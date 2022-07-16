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
        }
        return weaponParts;
    }

    public static void InstantiateWeaponParts(WeaponPart[] weaponParts, Vector3 position, Quaternion rotation)
    {
        foreach (WeaponPart part in weaponParts)
        {
            var wepInstance = Instantiate(part.weaponGameObject);
            wepInstance.transform.position = position;
            wepInstance.transform.rotation = rotation * Quaternion.Euler(0f, 0f, part.pre_direction);

            part.UpdateValues(wepInstance);
        }
    }
}

public enum WeaponType
{
    Projectile,
    Single,
    Sphere,
    Cone,
    Line,
    Conjure
}

public enum TargetType
{
    TowardsMouse,
    Self,
    TowardsNearestEnemy,
    TowardsNearestMouse
}

public enum CollisionType
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

    public void UpdateValues(GameObject wepInstance)
    {
        switch (this)
        {
            case ProjectilePart proj:
                proj.UpdateValues(wepInstance.GetComponent<ProjectileController>());
                break;
            case SpherePart sphere:
                sphere.UpdateValues(wepInstance.GetComponent<SphereController>());
                break;
        }
    }
}
