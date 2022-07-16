using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sphere", menuName = "Weapons/Custom Sphere"), Serializable]
public class SphereWepSO : WeaponSO
{
    [NonReorderable]
    public SpherePart[] weaponParts;
}

[Serializable]
public class SpherePart : WeaponPart
{
    [Header("Sphere Properties")]
    [Range(0, 5)] public float cur_lifeTime;
    [Range(0, 20)] public float cur_speed;
    public TargetType cur_targetType;

    [Header("Pre-Attack")]
    [Range(0, 5)] public float pre_lifeTime;

    [Header("Post-Attack")]
    public WeaponSO post_subWeapon;


    public void UpdateValues(SphereController instance)
    {
        instance.damage = this.damage;
        instance.collisionType = this.collisionType;
        instance.weaponType = this.weaponType;
        
        instance.cur_lifeTime = this.cur_lifeTime;
        instance.cur_speed = this.cur_speed;
        instance.cur_targetType = this.cur_targetType;

        instance.pre_lifeTime = this.pre_lifeTime;

        instance.post_subWeapon = this.post_subWeapon;
    }
}
