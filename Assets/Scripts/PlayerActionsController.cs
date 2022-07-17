using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerActionsController : MonoBehaviour
{
    private Camera MainCamera;
    private bool CanAttack;
    
    public WeaponSO equippedWeapon;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        CanAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && equippedWeapon is not null)
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        if (CanAttack)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        CanAttack = false;

        Vector2 playerPos = transform.position;
        Vector2 mousePos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        var dir = playerPos - mousePos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var rotToMouse = Quaternion.Euler(0f, 0f, angle);

        dynamic weaponParts = WeaponSO.ConvertWeaponToParts(equippedWeapon);

        for (int i = 0; i < equippedWeapon.amount; i++)
        {
            WeaponSO.InstantiateWeaponParts(weaponParts, transform.position, rotToMouse);

            if (equippedWeapon.amountBurstTime > 0f)
            {
                yield return new WaitForSeconds(equippedWeapon.amountBurstTime);
            }
        }
        yield return new WaitForSeconds(equippedWeapon.cooldown);
        CanAttack = true;
    }
}
