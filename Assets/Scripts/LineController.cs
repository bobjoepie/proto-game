using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    [Header("Properties")]
    public WeaponType weaponType;
    public CollisionType collisionType;
    public GameObject weaponGameObject;
    public int damage;
    public SpawnLocation weaponSpawn;

    [Header("Line Properties")]
    public bool cur_hasCollision;
    [Range(0, 5)] public float cur_lifeTime;
    [Range(0, 20)] public float cur_width;
    [Range(-360, 360)] public float cur_direction;
    [Range(0, 90)] public float cur_spread; //TODO
    [Range(-360, 360)] public float cur_rotation;
    [Range(0, 200)] public float cur_rotationSpeed; //TODO
    public TargetType cur_targetType; //TODO

    [Header("Pre-Attack")]
    [Range(0, 5)] public float pre_lifeTime;

    [Header("Post-Attack")]
    public WeaponSO post_subWeapon;

    private Camera MainCamera;

    private float ElapsedTime;

    public Quaternion initDir;
    public Vector2 initPos;
    public Vector2 initMousePos;

    public Vector2 destPos;

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

        // Line must spawn in start due to position issues with scaling, may need another method later
        switch (cur_targetType)
        {
            case TargetType.TowardsMouse:
                Vector2 bulletPos = transform.position;
                Vector2 mousePos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
                var dir = bulletPos - mousePos;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.parent.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

                var length2 = -1 * Vector2.Distance(bulletPos, mousePos);
                transform.parent.localScale = new Vector3(length2, cur_width, transform.localScale.z);
                break;
            case TargetType.TowardsInitialMouse:
                Vector2 bulletPos2 = transform.position;
                var dir2 = bulletPos2 - initMousePos;
                var angle2 = Mathf.Atan2(dir2.y, dir2.x) * Mathf.Rad2Deg;
                transform.parent.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle2));

                var length = -1 * Vector2.Distance(bulletPos2, initMousePos);
                transform.parent.localScale = new Vector3(length, cur_width, transform.localScale.z);
                break;
            case TargetType.None:
            default:
                transform.rotation = initDir * Quaternion.Euler(0f, 0f, cur_direction);
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

        switch (isPrepped)
        {
            case true when cur_hasCollision:
                ProcessPostAttack();
                break;
        }

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
        ElapsedTime += Time.deltaTime;
    }

    private void ProcessPostAttack()
    {
        Destroy(gameObject);
        dynamic weaponParts = WeaponSO.ConvertWeaponToParts(post_subWeapon);
        var lineEnd = GetComponentInChildren<LineEnd>().transform.position;
        WeaponSO.InstantiateWeaponParts(weaponParts, lineEnd, gameObject.transform.rotation);
    }

    private void InitAttack()
    {
        isPrepped = true;
    }
}
