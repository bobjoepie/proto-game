using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public bool CanAttack = true;
    public EntityController entityController;

    private void Start()
    {
        entityController = transform.root.GetComponent<EntityController>();
    }

    private void Update()
    {
        if (CanAttack)
        {
            CanAttack = false;
            TryAttack(entityController.actionRadiusController, entityController.appendages, true).Forget();
        }
    }
    private async UniTaskVoid TryAttack(ActionRadiusController actionRadiusController, List<AppendageController> appendages, bool isTargeted = true)
    {
        await TryPerformAttacks(actionRadiusController, appendages, isTargeted);
        await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
        CanAttack = true;
    }

    private async UniTask TryPerformAttacks(ActionRadiusController actionRadiusController, List<AppendageController> appendages, bool isTargeted = true)
    {
        foreach (AppendageController appendage in appendages.ToList())
        {
            if (isTargeted)
            {
                if (actionRadiusController == null || !actionRadiusController.targets.Any() ||
                    appendage == null) return;
                appendage.PerformAttacks(actionRadiusController.GetClosestTarget().position);
                await UniTask.Delay(TimeSpan.FromSeconds(0.15f));
            }
            else
            {
                //appendage.PerformAttacks(actionRadiusController.GetClosestTarget().position);
                await UniTask.Delay(TimeSpan.FromSeconds(0.15f));
            }
        }
    }
}
