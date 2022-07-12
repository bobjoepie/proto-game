using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionsController : MonoBehaviour
{
    public float attackCooldown;
    public ProjectileAttackScriptableObject projectile;
    public int bulletDamage;
    private Camera MainCamera;
    private bool CanAttack;

    public ProjectileAttackScriptableObject equippedProjectile;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        CanAttack = true;
        equippedProjectile = projectile;
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
        var projectileObj = equippedProjectile.attackParts[0].projectileGameObject;
        var projInstance = Instantiate(projectileObj);
        projInstance.transform.position = transform.position;
        projInstance.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        //projInstance.GetComponent<ProjectileController>().damage = bulletDamage;

        yield return new WaitForSeconds(attackCooldown);
        CanAttack = true;
    }
}
