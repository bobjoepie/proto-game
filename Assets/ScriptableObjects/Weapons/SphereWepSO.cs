using System;
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
    [Range(-360, 360)] public float cur_direction;
    public TargetType cur_targetType;
    public AudioClip cur_attackSound;

    [Header("Pre-Attack")]
    [Range(0, 5)] public float pre_lifeTime;
    public TargetType pre_targetType;
    public AudioClip pre_attackSound;

    [Header("Post-Attack")]
    public WeaponSO post_subWeapon;


    public void UpdateValues(SphereController instance, int iterationNum)
    {
        instance.iterationNum = iterationNum + 1;

        instance.damage = this.damage;
        instance.collisionType = this.collisionType;
        instance.weaponType = this.weaponType;
        instance.weaponSpawn = this.weaponSpawn;
        
        instance.cur_lifeTime = this.cur_lifeTime;
        instance.cur_speed = this.cur_speed;
        instance.cur_direction = this.cur_direction;
        instance.cur_targetType = this.cur_targetType;
        instance.cur_attackSound = this.cur_attackSound;

        instance.pre_lifeTime = this.pre_lifeTime;
        instance.pre_direction = this.pre_direction;
        instance.pre_targetType = this.pre_targetType;
        instance.pre_attackSound = this.pre_attackSound;

        instance.post_subWeapon = this.post_subWeapon;
    }
}
