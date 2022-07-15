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
        if (Input.GetKey(KeyCode.Mouse0) && equippedWeapon != null)
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
        Vector2 mousePos = (Vector2)MainCamera.ScreenToWorldPoint(Input.mousePosition);
        var dir = playerPos - mousePos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        switch (equippedWeapon)
        {
            case ProjectileWepSO projObj:
                var weaponParts = projObj.weaponParts;
                foreach (var part in weaponParts)
                {
                    var projInstance = Instantiate(part.weaponGameObject);
                    projInstance.transform.position = transform.position;
                    projInstance.transform.rotation = Quaternion.Euler(0f, 0f, angle + part.pre_direction);

                    part.UpdateValues(projInstance.GetComponent<ProjectileController>());
                }
                yield return new WaitForSeconds(projObj.cooldown);
                break;
        }
        CanAttack = true;
    }
}
