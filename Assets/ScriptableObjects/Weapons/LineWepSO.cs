using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Line", menuName = "Weapons/Custom Line"), Serializable]
public class LineWepSO : WeaponSO
{
    [NonReorderable]
    public LinePart[] weaponParts;
}

[Serializable]
public class LinePart : WeaponPart
{
    [Header("Line Properties")]
    public bool cur_hasCollision;
    [Range(0, 5)] public float cur_lifeTime;
    [Range(0, 20)] public float cur_maxDistance;
    [Range(0, 20)] public float cur_width;
    [Range(-360, 360)] public float cur_direction;
    [Range(0, 90)] public float cur_spread;
    [Range(-360, 360)] public float cur_rotation;
    [Range(0, 200)] public float cur_rotationSpeed;
    public TargetType cur_targetType;

    [Header("Pre-Attack")]
    //public bool pre_hasCollision;
    [Range(0, 5)] public float pre_lifeTime;
    //[Range(0, 20)] public float pre_speed;
    //[Range(0, 90)] public float pre_spread;
    //[Range(-360, 360)] public float pre_rotation;
    //[Range(0, 200)] public float pre_rotationSpeed;
    //public TargetType pre_targetType;

    [Header("Post-Attack")]
    public WeaponSO post_subWeapon;


    public void UpdateValues(LineController instance, int iterationNum)
    {
        instance.iterationNum = iterationNum + 1;

        instance.damage = this.damage;
        instance.collisionType = this.collisionType;
        instance.weaponType = this.weaponType;
        instance.weaponSpawn = this.weaponSpawn;

        instance.cur_hasCollision = this.cur_hasCollision;
        instance.cur_lifeTime = this.cur_lifeTime;
        instance.cur_maxDistance = this.cur_maxDistance;
        instance.cur_width = this.cur_width;
        instance.cur_direction = this.cur_direction;
        instance.cur_spread = this.cur_spread;
        instance.cur_rotation = this.cur_rotation;
        instance.cur_rotationSpeed = this.cur_rotationSpeed;
        instance.cur_targetType = this.cur_targetType;
        
        instance.pre_lifeTime = this.pre_lifeTime;

        instance.post_subWeapon = this.post_subWeapon;
    }
}
