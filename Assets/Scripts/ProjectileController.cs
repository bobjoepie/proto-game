using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public int damage;
    public float cooldown;
    public float rotation;
    public Camera mainCamera;

    public bool isHoming;
    public float prepSpeed;
    public float prepTime = 0f;

    private float ElapsedTime;
    
    public bool hasDestinationPos;
    private Vector2 destinationPos;

    //private Collider2D ProjectileCollider;
    // Start is called before the first frame update
    void Start()
    {
        ElapsedTime = 0f;
        mainCamera = Camera.main;
        destinationPos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (isHoming)
        {
            GetComponent<Collider2D>().enabled = false;
        }
        //ProjectileCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHoming)
        {
            var step = speed * Time.deltaTime;
            transform.position += -transform.right * step;
            if (rotation != 0f)
            {
                transform.Rotate(0, 0, rotation * Time.deltaTime);
            }
            ElapsedTime += Time.deltaTime;
        }
        else if (prepTime <= 0f)
        {
            if (!hasDestinationPos)
            {
                Vector2 bulletPos = transform.position;
                //Vector2 mousePos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
                var dir = bulletPos - destinationPos;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

                GetComponent<Collider2D>().enabled = true;
                hasDestinationPos = true;
            }

            var step = speed * Time.deltaTime;
            transform.position += -transform.right * step;
            ElapsedTime += Time.deltaTime;
        }
        else
        {
            var step = prepSpeed * Time.deltaTime;
            transform.position += -transform.right * step;
            if (rotation != 0f)
            {
                transform.Rotate(0, 0, rotation * Time.deltaTime);
            }
            prepTime -= Time.deltaTime;
        }

        if (ElapsedTime > lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Uncollidable") || col.gameObject.CompareTag("Projectile")) return;
        var closestPoint = col.ClosestPoint(transform.position);
        col.gameObject.GetComponent<EnemyController>()?.TakeDamage(damage, closestPoint);
        Destroy(gameObject);
    }
}
