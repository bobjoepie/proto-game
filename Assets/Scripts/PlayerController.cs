using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class InventoryItem
{
    public Sprite sprite;
    public Color color;
    public string name;
    public int amount;
    public CustomProjectileAttack projectile2;
}

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
    private List<string> pickupList;
    private List<InventoryItem> inventoryList;

    [SerializeField] 
    private InventoryItem EquippedItem;
    private PlayerActionsController PlayerActions;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        origPos = childSprite.transform.localPosition;
        health = maxHealth;

        childSpriteRenderer = childSprite.GetComponent<SpriteRenderer>();
        origColor = childSpriteRenderer.color;

        pickupList = new List<string>();
        inventoryList = new List<InventoryItem>();

        PlayerActions = GetComponent<PlayerActionsController>();
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        transform.position += new Vector3(horizontal * speed * Time.deltaTime, vertical * speed * Time.deltaTime, 0);

        CheckEquipmentInputs();
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

    public void PickupItem2(string item, SpriteRenderer spriteRenderer, CustomProjectileAttack projectilePickup = null)
    {
        //pickupList.Add(item);
        Debug.Log($"Picked up {projectilePickup.name}");
        //GameMenuManager.Instance.SetInventoryItem(0, item, spriteRenderer);
        var newItem = new InventoryItem
        {
            name = projectilePickup.name,
            sprite = spriteRenderer.sprite,
            color = spriteRenderer.color,
            amount = 1,
            projectile2 = projectilePickup
        };
        if (inventoryList.Select(i => i.name).ToList().Contains(newItem.name)) return;
        inventoryList.Add(newItem);
        GameMenuManager.Instance.SetInventoryItem2(inventoryList);
    }

    public void CheckEquipmentInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameMenuManager.Instance.ClearSelectedItem();
            EquippedItem = null;
            if (inventoryList.ElementAtOrDefault(0) != null)
            {
                EquippedItem = inventoryList[0];
                GameMenuManager.Instance.SelectItem(0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameMenuManager.Instance.ClearSelectedItem();
            EquippedItem = null;
            if (inventoryList.ElementAtOrDefault(1) != null)
            {
                EquippedItem = inventoryList[1];
                GameMenuManager.Instance.SelectItem(1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameMenuManager.Instance.ClearSelectedItem();
            EquippedItem = null;
            if (inventoryList.ElementAtOrDefault(2) != null)
            {
                EquippedItem = inventoryList[2];
                GameMenuManager.Instance.SelectItem(2);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GameMenuManager.Instance.ClearSelectedItem();
            EquippedItem = null;
            if (inventoryList.ElementAtOrDefault(3) != null)
            {
                EquippedItem = inventoryList[3];
                GameMenuManager.Instance.SelectItem(3);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GameMenuManager.Instance.ClearSelectedItem();
            EquippedItem = null;
            if (inventoryList.ElementAtOrDefault(4) != null)
            {
                EquippedItem = inventoryList[4];
                GameMenuManager.Instance.SelectItem(4);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            GameMenuManager.Instance.ClearSelectedItem();
            EquippedItem = null;
            if (inventoryList.ElementAtOrDefault(5) != null)
            {
                EquippedItem = inventoryList[5];
                GameMenuManager.Instance.SelectItem(5);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            GameMenuManager.Instance.ClearSelectedItem();
            EquippedItem = null;
            if (inventoryList.ElementAtOrDefault(6) != null)
            {
                EquippedItem = inventoryList[6];
                GameMenuManager.Instance.SelectItem(6);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            GameMenuManager.Instance.ClearSelectedItem();
            EquippedItem = null;
            if (inventoryList.ElementAtOrDefault(7) != null)
            {
                EquippedItem = inventoryList[7];
                GameMenuManager.Instance.SelectItem(7);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            GameMenuManager.Instance.ClearSelectedItem();
            EquippedItem = null;
            if (inventoryList.ElementAtOrDefault(8) != null)
            {
                EquippedItem = inventoryList[8];
                GameMenuManager.Instance.SelectItem(8);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            GameMenuManager.Instance.ClearSelectedItem();
            EquippedItem = null;
            if (inventoryList.ElementAtOrDefault(9) != null)
            {
                EquippedItem = inventoryList[9];
                GameMenuManager.Instance.SelectItem(9);
            }
        }
        else
        {
            return;
        }

        PlayerActions.equippedProjectile2 = null;
        if (EquippedItem != null)
        {
            if (EquippedItem.projectile2 != null)
            {
                PlayerActions.equippedProjectile2 = EquippedItem.projectile2;
                Debug.Log("projectile equipped");
            }
            Debug.Log($"Equipped {EquippedItem.name}");
        }
    }

}
