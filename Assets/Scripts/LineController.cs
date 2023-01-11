using UnityEngine;

public class LineController : MonoBehaviour
{
    [Header("Properties")]
    public WeaponType weaponType;
    public CollisionType collisionType;
    public GameObject weaponGameObject;
    public Transform lineEnd;
    public int damage;
    public SpawnLocation weaponSpawn;
    public int iterationNum;

    [Header("Line Properties")]
    public bool cur_hasCollision;
    [Range(0, 5)] public float cur_lifeTime;
    [Range(0, 20)] public float cur_maxDistance;
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
        Vector2 bulletPos;
        Vector2 destPos;
        Vector2 dir;
        float angle;
        float length;
        // Line must spawn in start due to position issues with scaling, may need another method later
        switch (cur_targetType)
        {
            case TargetType.TowardsMouse:
                bulletPos = transform.position;
                destPos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
                dir = bulletPos - destPos;
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.parent.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

                length = -1 * Vector2.Distance(bulletPos, destPos);
                transform.parent.localScale = new Vector3(length, cur_width, transform.localScale.z);
                break;
            case TargetType.TowardsInitialMouse:
                bulletPos = transform.position;
                dir = bulletPos - initMousePos;
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.parent.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

                length = -1 * Vector2.Distance(bulletPos, initMousePos);
                transform.parent.localScale = new Vector3(length, cur_width, transform.localScale.z);
                break;
            case TargetType.None:
            default:
                
                transform.rotation = initDir * Quaternion.Euler(0f, 0f, cur_direction);
                var mask = 1 << LayerMask.GetMask("Player", "PlayerProjectile");
                var hit = Physics2D.CircleCast(transform.position, cur_width/2, -transform.right, cur_maxDistance, mask);

                if (hit.collider != null)
                {
                    length = -1 * Vector2.Distance(transform.position, hit.point);
                    transform.parent.localScale = new Vector3(length, cur_width, transform.localScale.z);
                }
                else
                {
                    length = -1 * cur_maxDistance;
                    transform.parent.localScale = new Vector3(length, cur_width, transform.localScale.z);
                }
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
        col.gameObject.GetComponent<HitboxController>()?.TakeDamage(damage);

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
        Destroy(transform.parent.gameObject);
        dynamic weaponParts = WeaponSO.ConvertWeaponToParts(post_subWeapon);
        WeaponSO.InstantiateWeaponParts(weaponParts, lineEnd.position, gameObject.transform.rotation, gameObject.layer, iterationNum);
    }

    private void InitAttack()
    {
        isPrepped = true;
    }
}
