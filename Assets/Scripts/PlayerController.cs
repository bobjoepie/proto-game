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
    public WeaponSO weapon;
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
    private List<InventoryItem> inventoryList;

    [SerializeField] 
    private InventoryItem EquippedItem;
    private PlayerActionsController PlayerActions;

    private readonly Dictionary<int, KeyCode> inventoryKeys = new()
    {
        { 0, KeyCode.Alpha1 },
        { 1, KeyCode.Alpha2 },
        { 2, KeyCode.Alpha3 },
        { 3, KeyCode.Alpha4 },
        { 4, KeyCode.Alpha5 },
        { 5, KeyCode.Alpha6 },
        { 6, KeyCode.Alpha7 },
        { 7, KeyCode.Alpha8 },
        { 8, KeyCode.Alpha9 },
        { 9, KeyCode.Alpha0 }
    };

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        origPos = childSprite.transform.localPosition;
        health = maxHealth;

        childSpriteRenderer = childSprite.GetComponent<SpriteRenderer>();
        origColor = childSpriteRenderer.color;
        
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

    public void PickupItem(GameObject item, WeaponSO weaponPickup)
    {
        Debug.Log($"Picked up {weaponPickup.name}");
        var spriteRenderer = item.GetComponent<SpriteRenderer>();
        var newItem = new InventoryItem
        {
            name = weaponPickup.name,
            sprite = spriteRenderer.sprite,
            color = spriteRenderer.color,
            amount = 1,
            weapon = weaponPickup
        };
        if (inventoryList.Select(i => i.name).ToList().Contains(newItem.name)) return;
        inventoryList.Add(newItem);
        GameMenuManager.Instance.SetInventoryItem(inventoryList);
        Destroy(item);
    }

    private void CheckEquipmentInputs()
    {
        bool foundKey = false;
        foreach (var kvp in inventoryKeys)
        {
            if (Input.GetKeyDown(kvp.Value))
            {
                CheckNum(kvp.Key);
                foundKey = true;
                break;
            }
        }

        if (!foundKey) return;

        PlayerActions.equippedWeapon = null;
        if (EquippedItem != null)
        {
            if (EquippedItem.weapon != null)
            {
                PlayerActions.equippedWeapon = EquippedItem.weapon;
            }
            Debug.Log($"Equipped {EquippedItem.name}");
        }
    }

    private void CheckNum(int i)
    {
        GameMenuManager.Instance.ClearSelectedItem();
        EquippedItem = null;
        if (inventoryList.ElementAtOrDefault(i) != null)
        {
            EquippedItem = inventoryList[i];
            GameMenuManager.Instance.SelectItem(i);
        }
    }

}
