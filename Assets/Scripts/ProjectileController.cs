using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [Header("Properties")]
    public WeaponType weaponType;
    public CollisionType collisionType;
    public GameObject weaponGameObject;
    public int damage;
    public SpawnLocation weaponSpawn;
    public int iterationNum;

    [Header("Projectile Properties")]
    public bool cur_hasCollision;
    [Range(0, 5)] public float cur_lifeTime;
    [Range(0, 20)] public float cur_speed;
    [Range(-360, 360)] public float cur_direction;
    [Range(0, 90)] public float cur_spread; //TODO
    [Range(-360, 360)] public float cur_rotation;
    [Range(0, 200)] public float cur_rotationSpeed; //TODO
    public TargetType cur_targetType; //TODO

    [Header("Pre-Attack")]
    public bool pre_hasCollision; //TODO
    [Range(0, 5)] public float pre_lifeTime;
    [Range(0, 20)] public float pre_speed;
    [Range(-360, 360)] public float pre_direction; //TODO: move to WeaponController.cs file
    [Range(0, 90)] public float pre_spread; //TODO
    [Range(-360, 360)] public float pre_rotation;
    [Range(0, 200)] public float pre_rotationSpeed; //TODO
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
        var closestPoint = col.ClosestPoint(transform.position);
        if (col.TryGetComponent<HitboxController>(out var hitbox)) hitbox.TakeDamage(damage);

        switch (isPrepped)
        {
            case true when cur_hasCollision:
            case false when pre_hasCollision:
                ProcessPostAttack();
                break;
        }
        
    }

    private void ProcessPreAttack()
    {
        var step = pre_speed * Time.deltaTime;
        transform.position += -transform.right * step;
        if (pre_rotation != 0f)
        {
            transform.Rotate(0, 0, pre_rotation * Time.deltaTime);
        }

        if (pre_lifeTime <= 0f)
        {
            InitAttack();
        }
        pre_lifeTime -= Time.deltaTime;
    }

    private void ProcessAttack()
    {
        var step = cur_speed * Time.deltaTime;
        transform.position += -transform.right * step;
        if (cur_rotation != 0f)
        {
            transform.Rotate(0, 0, cur_rotation * Time.deltaTime);
        }
        ElapsedTime += Time.deltaTime;
    }

    private void ProcessPostAttack()
    {
        Destroy(gameObject);
        dynamic weaponParts = WeaponSO.ConvertWeaponToParts(post_subWeapon);
        WeaponSO.InstantiateWeaponParts(weaponParts, gameObject.transform.position, gameObject.transform.rotation, gameObject.layer, iterationNum);
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
