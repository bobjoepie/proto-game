using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    [Header("Properties")]
    public WeaponType weaponType;
    public CollisionType collisionType;
    public GameObject weaponGameObject;
    public int damage;
    public SpawnLocation weaponSpawn;
    public int iterationNum;

    [Header("Sphere Properties")]
    [Range(0, 5)] public float cur_lifeTime;
    [Range(0, 20)] public float cur_speed;
    [Range(-360, 360)] public float cur_direction;
    public TargetType cur_targetType;

    [Header("Pre-Attack")]
    [Range(0, 5)] public float pre_lifeTime;
    [Range(-360, 360)] public float pre_direction;
    public TargetType pre_targetType;

    [Header("Post-Attack")]
    public WeaponSO post_subWeapon;

    private Camera MainCamera;

    private float ElapsedTime;

    public Quaternion initDir;
    public Vector2 initPos;
    public Vector2 initMousePos;

    private bool isPrepped;

    // Start is called before the first frame update
    void Start()
    {
        ElapsedTime = 0f;
        MainCamera = Camera.main;
        initMousePos = MainCamera.ScreenToWorldPoint(Input.mousePosition);

        switch (weaponSpawn)
        {
            case SpawnLocation.MousePoint:
                transform.position = initMousePos;
                break;
            case SpawnLocation.None:
            default:
                break;
        }

        initDir = transform.rotation;
        initPos = transform.position;
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
            InitAttack();
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
        WeaponSO.InstantiateWeaponParts(weaponParts, gameObject.transform.position, gameObject.transform.rotation, iterationNum);
    }

    private void InitAttack()
    {
        isPrepped = true;
        switch (cur_targetType)
        {
            case TargetType.TowardsMouse:
                Vector2 bulletPos = transform.position;
                Vector2 mousePos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
                var dir = bulletPos - mousePos;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
                break;
            case TargetType.TowardsInitialMouse:
                Vector2 bulletPos2 = transform.position;
                var dir2 = bulletPos2 - initMousePos;
                var angle2 = Mathf.Atan2(dir2.y, dir2.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle2));
                break;
            case TargetType.None:
            default:
                transform.rotation = initDir * Quaternion.Euler(0f, 0f, cur_direction);
                break;
        }
    }
}
