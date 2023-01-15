using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public WeaponSO weaponSO;
    public dynamic weaponParts;
    public LayerMask mask;

    private PlayerController playerController;
    public bool CanAttack = true;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        if (weaponParts == null && weaponSO != null)
        {
            weaponParts = WeaponSO.ConvertWeaponToParts(weaponSO);
        }
    }

    private void OnEnable()
    {
        GetComponent<AppendageController>()?.Register(this);
    }

    public async UniTaskVoid Attack(Vector2 targetPos)
    {
        if (!CanAttack) return;
        CanAttack = false;

        if (playerController != null && playerController.equippedWeapon != weaponSO)
        {
            weaponSO = playerController.equippedWeapon;
            weaponParts = WeaponSO.ConvertWeaponToParts(playerController.equippedWeapon);
        }

        for (int i = 0; i < weaponSO.amount; i++)
        {
            WeaponSO.InstantiateWeaponParts(weaponParts, transform.position, transform.position.AngleTowards2D(targetPos), mask.ToLayer());
            if (weaponSO.amountBurstTime > 0f)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(weaponSO.amountBurstTime));
            }
        }

        await UniTask.Delay(TimeSpan.FromSeconds(weaponSO.cooldown));
        CanAttack = true;
    }
}
