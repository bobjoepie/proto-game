using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject childSprite;
    public int health;
    public float knockbackDist;

    private Vector2 origPos;
    private Color origColor;
    private SpriteRenderer childSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        origPos = childSprite.transform.localPosition;

        childSpriteRenderer = childSprite.GetComponent<SpriteRenderer>();
        origColor = childSpriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage, Vector2 closestPoint)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(SpriteKnockback(closestPoint));
        }
    }

    IEnumerator SpriteKnockback(Vector2 fromPoint)
    {
        Vector2 enemyPos = transform.position;
        var dir = fromPoint - enemyPos;

        childSprite.transform.position += new Vector3(-dir.x, -dir.y, 0) * knockbackDist;
        childSpriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.05f);
        childSprite.transform.localPosition = origPos;
        childSpriteRenderer.color = origColor;
    }
}
