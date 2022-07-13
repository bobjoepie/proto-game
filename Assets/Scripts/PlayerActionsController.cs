using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerActionsController : MonoBehaviour
{
    private Camera MainCamera;
    private bool CanAttack;

    public ProjectileAttackScriptableObject equippedProjectile;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        CanAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && equippedProjectile != null)
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

        var projObj = equippedProjectile.attackParts[0];
        var projInstance = Instantiate(projObj.projectileGameObject);
        projInstance.transform.position = transform.position;
        projInstance.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        var projProp = projInstance.GetComponent<ProjectileController>();
        projProp.damage = projObj.damage;
        projProp.speed = projObj.speed;
        projProp.lifeTime = projObj.lifeTime;
        projProp.cooldown = projObj.cooldown;
        projProp.rotation = projObj.rotation;

        yield return new WaitForSeconds(projProp.cooldown);
        CanAttack = true;
    }
}
