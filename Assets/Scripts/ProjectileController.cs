using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public int damage;

    private float ElapsedTime;
    private Collider2D ProjectileCollider;
    // Start is called before the first frame update
    void Start()
    {
        ElapsedTime = 0f;
        ProjectileCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var step = speed * Time.deltaTime;
        transform.position += -transform.right * step;
        ElapsedTime += Time.deltaTime;
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
