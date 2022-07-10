using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float speed;
    public int health;
    public int maxHealth;
    public float knockbackDist;
    public GameObject childSprite;

    private Rigidbody2D rb;
    private Vector2 origPos;
    private Color origColor;
    private SpriteRenderer childSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        origPos = childSprite.transform.localPosition;
        health = maxHealth;

        childSpriteRenderer = childSprite.GetComponent<SpriteRenderer>();
        origColor = childSpriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        transform.position += new Vector3(horizontal * speed * Time.deltaTime, vertical * speed * Time.deltaTime, 0);
    }

    public void TakeDamage(int damage, Vector2 closestPoint)
    {
        health -= damage;
        GameMenuManager.Instance.SetHealthBar(health, maxHealth);
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
