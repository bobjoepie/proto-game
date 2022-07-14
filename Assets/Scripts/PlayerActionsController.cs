using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerActionsController : MonoBehaviour
{
    private Camera MainCamera;
    private bool CanAttack;
    
    public CustomProjectileAttack equippedProjectile2;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        CanAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && equippedProjectile2 != null)
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

        //var projObj = equippedProjectile.attackParts[0];
        //var projInstance = Instantiate(projObj.projectileGameObject);
        //projInstance.transform.position = transform.position;
        //projInstance.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        //var projProp = projInstance.GetComponent<ProjectileController>();
        //projProp.damage = projObj.damage;
        //projProp.speed = projObj.speed;
        //projProp.lifeTime = projObj.lifeTime;
        //projProp.cooldown = projObj.cooldown;
        //projProp.rotation = projObj.rotation;

        var projObj = equippedProjectile2;
        var cooldown = projObj.cooldown;
        var attackParts = equippedProjectile2.attackParts;
        foreach (var part in attackParts)
        {
            var projInstance = Instantiate(part.projectileGameObject);
            projInstance.transform.position = transform.position;
            projInstance.transform.rotation = Quaternion.Euler(0f, 0f, angle + part.direction);
            var projProp = projInstance.GetComponent<ProjectileController>();
            projProp.damage = part.damage;
            projProp.speed = part.speed;
            projProp.lifeTime = part.lifeTime;
            projProp.rotation = part.rotation;

            projProp.isHoming = part.homing;
            projProp.prepTime = part.prepTime;
            projProp.prepSpeed = part.prepSpeed;
        }

        yield return new WaitForSeconds(cooldown);
        CanAttack = true;
    }
}
