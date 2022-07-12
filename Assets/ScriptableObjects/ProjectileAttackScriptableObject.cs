using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AttackPart
{
    //[NonReorderable]
    //public List<AttackPart> attackParts;
    public GameObject projectileGameObject;
    public int amount;
}

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Attack")]
public class ProjectileAttackScriptableObject : ScriptableObject
{
    [NonReorderable]
    public List<AttackPart> attackParts;
}
