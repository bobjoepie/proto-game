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
    public PickupSO pickupObj;
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
    private InventoryItem[] inventoryList;

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

        inventoryList = new InventoryItem[10];

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

    public void Pickup(GameObject item, PickupSO pickupObj)
    {
        var spriteRenderer = item.GetComponentInChildren<SpriteRenderer>();
        InventoryItem newItem;
        switch (pickupObj)
        {
            case WeaponSO weaponObj:
                newItem = new InventoryItem
                {
                    name = weaponObj.name,
                    sprite = spriteRenderer.sprite,
                    color = spriteRenderer.color,
                    amount = 1,
                    pickupObj = weaponObj,
                };
                break;
            case ItemSO itemObj:
                newItem = new InventoryItem
                {
                    name = itemObj.name,
                    sprite = spriteRenderer.sprite,
                    color = spriteRenderer.color,
                    amount = 1,
                    pickupObj = itemObj,
                };
                break;
            default:
                return;
        }
        
        if (inventoryList.Where(i => i != null).Select(it => it.name).ToList().Contains(newItem.name)) return;

        bool pickedUp = false;
        for (int i = 0; i < inventoryList.Length; i++)
        {
            if (inventoryList[i] == null)
            {
                inventoryList[i] = newItem;
                Debug.Log($"Picked up {pickupObj.name}");
                GameMenuManager.Instance.SetInventoryItem(inventoryList);
                Destroy(item);
                pickedUp = true;
                break;
            }
        }

        if (!pickedUp)
        {
            Debug.Log("Inventory is full!");
        }
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
        PlayerActions.equippedItem = null;
        if (EquippedItem == null) return;

        if (EquippedItem.pickupObj != null)
        {
            switch (EquippedItem.pickupObj)
            {
                case WeaponSO weaponObj:
                    PlayerActions.equippedWeapon = weaponObj;
                    break;
                case ItemSO itemObj:
                    PlayerActions.equippedItem = itemObj;
                    break;
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
