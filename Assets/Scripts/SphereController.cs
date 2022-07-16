using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    private Camera MainCamera;

    [Header("Properties")]
    public WeaponType weaponType;
    public CollisionType collisionType;
    public GameObject weaponGameObject;
    public int damage;

    [Header("Sphere Properties")]
    [Range(0, 5)] public float cur_lifeTime;
    [Range(0, 20)] public float cur_speed;
    public TargetType cur_targetType;

    [Header("Pre-Attack")]
    [Range(0, 5)] public float pre_lifeTime;

    [Header("Post-Attack")]
    public WeaponSO post_subWeapon;

    private float ElapsedTime;

    public Quaternion initDir;
    public Vector2 initPos;

    private bool isPrepped;
    private bool isFinished;

    // Start is called before the first frame update
    void Start()
    {
        ElapsedTime = 0f;

        initDir = transform.rotation;
        initPos = transform.position;

        MainCamera = Camera.main;

        switch (cur_targetType)
        {
            case TargetType.Self:
                break;
            case TargetType.TowardsMouse:
            default:
                transform.position = (Vector2)MainCamera.ScreenToWorldPoint(Input.mousePosition);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPrepped)
        {
            ProcessPreAttack();
        }
        if (!isPrepped) return;
        ProcessAttack();

        if (ElapsedTime > cur_lifeTime)
        {
            ProcessPostAttack();
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (/*col.gameObject.CompareTag("Player") || */col.gameObject.CompareTag("Uncollidable") || col.gameObject.CompareTag("Projectile")) return;
        var closestPoint = col.ClosestPoint(transform.position);
        col.gameObject.GetComponent<EnemyController>()?.TakeDamage(damage, closestPoint);
        col.gameObject.GetComponent<PlayerController>()?.TakeDamage(damage, closestPoint);
    }

    private void ProcessPreAttack()
    {
        if (pre_lifeTime <= 0f)
        {
            isPrepped = true;
        }
        pre_lifeTime -= Time.deltaTime;
    }

    private void ProcessAttack()
    {
        var step = cur_speed * Time.deltaTime;
        transform.localScale += new Vector3(step, step, 0f);
        ElapsedTime += Time.deltaTime;
    }

    private void ProcessPostAttack()
    {
        Destroy(gameObject);
        dynamic weaponParts = WeaponSO.ConvertWeaponToParts(post_subWeapon);
        WeaponSO.InstantiateWeaponParts(weaponParts, gameObject.transform.position, gameObject.transform.rotation);
    }
}
