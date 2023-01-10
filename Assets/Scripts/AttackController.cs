using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public WeaponSO weaponSO;
    public dynamic weaponParts;

    private void Awake()
    {
        weaponParts = WeaponSO.ConvertWeaponToParts(weaponSO);
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AppendageController>().Register(this);
    }

    public async UniTaskVoid Attack(Vector2 targetPos)
    {
        for (int i = 0; i < weaponSO.amount; i++)
        {
            WeaponSO.InstantiateWeaponParts(weaponParts, transform.position, transform.position.AngleTowards2D(targetPos));

            if (weaponSO.amountBurstTime > 0f)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(weaponSO.amountBurstTime));
            }
        }
    }
}
