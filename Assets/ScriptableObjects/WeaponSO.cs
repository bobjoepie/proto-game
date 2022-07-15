using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Custom Weapon"), Serializable]
public abstract class WeaponSO : ScriptableObject
{
    public string name;
    [TextArea(3, 5)]
    public string description;
    [Range(0, 1)]
    public float cooldown;
}

public enum WeaponType
{
    Projectile,
    Single,
    Sphere,
    Cone,
    Line
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
}