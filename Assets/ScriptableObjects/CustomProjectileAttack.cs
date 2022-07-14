using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CustomAttackPart
{
    public string label;
    public GameObject projectileGameObject;
    public int damage;
    [Range(0,20)]
    public float speed;
    [Range(0,5)]
    public float lifeTime;
    [Range(-360,360)]
    public float direction;
    [Range(-360, 360)]
    public float rotation;
    public bool homing;
    public float prepTime;
    public float prepSpeed;
}

[CreateAssetMenu(fileName = "Custom Attack", menuName = "ScriptableObjects/Custom Attack")]
public class CustomProjectileAttack : ScriptableObject
{
    public string name;
    [TextArea(3, 5)]
    public string description;
    [Range(0, 1)]
    public float cooldown;
    [NonReorderable]
    public List<CustomAttackPart> attackParts;
}
