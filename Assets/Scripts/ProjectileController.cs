using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [Header("Properties")]
    public WeaponType weaponType;
    public CollisionType collisionType;
    public GameObject weaponGameObject;
    public int damage;

    [Header("Projectile Properties")]
    public bool cur_hasCollision;
    [Range(0, 5)] public float cur_lifeTime;
    [Range(0, 20)] public float cur_speed;
    [Range(-360, 360)] public float cur_direction;
    [Range(0, 90)] public float cur_spread;
    [Range(-360, 360)] public float cur_rotation;
    [Range(0, 200)] public float cur_rotationSpeed;
    public TargetType cur_targetType;

    [Header("Pre-Attack")]
    public bool pre_hasCollision;
    [Range(0, 5)] public float pre_lifeTime;
    [Range(0, 20)] public float pre_speed;
    [Range(-360, 360)] public float pre_direction;
    [Range(0, 90)] public float pre_spread;
    [Range(-360, 360)] public float pre_rotation;
    [Range(0, 200)] public float pre_rotationSpeed;

    [Header("Post-Attack")]
    public WeaponSO post_subWeapon;

    public Camera mainCamera;

    private float ElapsedTime;
    
    public bool hasDestinationPos;
    private Vector2 destinationPos;

    public Quaternion initDir;
    public Vector2 initPos;

    private bool isPrepped;
    private bool isFinished;
    
    // Start is called before the first frame update
    void Start()
    {
        ElapsedTime = 0f;
        mainCamera = Camera.main;
        destinationPos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //GetComponent<Collider2D>().enabled = pre_hasCollision;

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
        if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Uncollidable") || col.gameObject.CompareTag("Projectile")) return;
        var closestPoint = col.ClosestPoint(transform.position);
        col.gameObject.GetComponent<EnemyController>()?.TakeDamage(damage, closestPoint);

        if (isPrepped && cur_hasCollision)
        {
            ProcessPostAttack();
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
            isPrepped = true;
            transform.rotation = initDir * Quaternion.Euler(0f, 0f,cur_direction);
        }
        pre_lifeTime -= Time.deltaTime;
    }

    private void ProcessAttack()
    {
        if (cur_targetType != TargetType.TowardsNearestMouse)
        {
            if (cur_rotation != 0f)
            {
                transform.Rotate(0, 0, cur_rotation * Time.deltaTime);
            }
        }
        else
        {
            if (!hasDestinationPos)
            {
                Vector2 bulletPos = transform.position;
                var dir = bulletPos - destinationPos;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

                GetComponent<Collider2D>().enabled = true;
                hasDestinationPos = true;
            }
        }

        var step = cur_speed * Time.deltaTime;
        transform.position += -transform.right * step;
        ElapsedTime += Time.deltaTime;
    }

    private void ProcessPostAttack()
    {
        Destroy(gameObject);
        dynamic weaponParts = WeaponSO.ConvertWeaponToParts(post_subWeapon);
        WeaponSO.InstantiateWeaponParts(weaponParts, gameObject.transform.position, gameObject.transform.rotation);
    }
}
