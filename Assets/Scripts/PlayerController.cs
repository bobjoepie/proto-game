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
    public string internalName;
    public int quantity;
    public PickupSO pickupObj;
}

public class PlayerController : MonoBehaviour
{
    public float speed;
    public int health;
    public int maxHealth;
    public float cooldownRate = 1;
    public float knockbackDist;
    public GameObject childSprite;
    public int curLevel;
    public int curExperience;
    public int expToLevel;

    private Rigidbody2D rb;
    public Collider2D playerCollider;
    private Vector2 origPos;
    private Color origColor;
    private SpriteRenderer childSpriteRenderer;
    private InventoryItem[] inventoryList;

    [SerializeField] 
    private InventoryItem ActiveInventoryItem;

    [SerializeField]
    public WeaponSO equippedWeapon;
    public ItemSO equippedItem;

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
        playerCollider = GetComponent<Collider2D>();
        origPos = childSprite.transform.localPosition;
        health = maxHealth;

        childSpriteRenderer = childSprite.GetComponent<SpriteRenderer>();
        origColor = childSpriteRenderer.color;

        inventoryList = new InventoryItem[10];
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        transform.position += new Vector3(horizontal * speed * Time.deltaTime, vertical * speed * Time.deltaTime, 0);

        CheckEquipmentInputs();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameMenuManager.Instance.ToggleInventoryWindow();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameMenuManager.Instance.TogglePauseMenu();
        }
    }

    public void TakeDamage(int damage, Vector2 closestPoint)
    {
        health -= damage;
        GameMenuManager.Instance.SetHealthBar(health, maxHealth);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        else if (damage > 0)
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

    public void Pickup(GameObject item, PickupSO pickupObj, Interactable interactableObj = null)
    {
        var spriteRenderer = item.GetComponentInChildren<SpriteRenderer>();
        InventoryItem newItem;
        switch (pickupObj)
        {
            case WeaponSO weaponObj:
                newItem = new InventoryItem
                {
                    name = weaponObj.name,
                    internalName = weaponObj.internalName,
                    sprite = spriteRenderer.sprite,
                    color = spriteRenderer.color,
                    quantity = 1,
                    pickupObj = weaponObj,
                };
                break;
            case ItemSO itemObj:
                newItem = new InventoryItem
                {
                    name = itemObj.name,
                    internalName = itemObj.internalName,
                    sprite = spriteRenderer.sprite,
                    color = spriteRenderer.color,
                    quantity = 1,
                    pickupObj = itemObj,
                };
                break;
            case ExpSO expObj:
                curExperience += expObj.amount;
                if (curExperience >= expToLevel)
                {
                    HandleLevelUp();
                }
                GameMenuManager.Instance.ExperienceBar.SetEXPBar(curExperience,expToLevel);
                if (interactableObj != null)
                {
                    interactableObj.quantity -= 1;
                    if (interactableObj.quantity <= 0)
                    {
                        Destroy(item);
                    }
                }
                else
                {
                    Destroy(item);
                }
                
                return;
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

    public void Pickup2(Interactable interactableObj)
    {
        var spriteRenderer = interactableObj.GetComponentInChildren<SpriteRenderer>();
        InventoryItem newItem;
        switch (interactableObj.pickupSO)
        {
            case WeaponSO weaponObj:
                newItem = new InventoryItem
                {
                    name = weaponObj.name,
                    internalName = weaponObj.internalName,
                    sprite = weaponObj.pickupSprite,
                    color = Color.white,
                    quantity = 1,
                    pickupObj = weaponObj,
                };
                break;
            case ItemSO itemObj:
                newItem = new InventoryItem
                {
                    name = itemObj.name,
                    internalName = itemObj.internalName,
                    sprite = itemObj.pickupSprite,
                    color = Color.white,
                    quantity = 1,
                    pickupObj = itemObj,
                };
                break;
            case ExpSO expObj:
                curExperience += expObj.amount;
                if (curExperience >= expToLevel)
                {
                    HandleLevelUp();
                }
                GameMenuManager.Instance.ExperienceBar.SetEXPBar(curExperience, expToLevel);
                return;
            default:
                return;
        }
        
        interactableObj.quantity -= 1;
        if (interactableObj.quantity <= 0)
        {
            Destroy(interactableObj.gameObject);
        }

        if (inventoryList.Where(i => i != null).Select(it => it.name).ToList().Contains(newItem.name)) return;

        bool pickedUp = false;
        for (int i = 0; i < inventoryList.Length; i++)
        {
            if (inventoryList[i] == null)
            {
                inventoryList[i] = newItem;
                Debug.Log($"Picked up {interactableObj.pickupSO.name}");
                GameMenuManager.Instance.SetInventoryItem(inventoryList);
                Destroy(interactableObj);
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

        equippedWeapon = null;
        equippedItem = null;
        if (ActiveInventoryItem == null) return;

        if (ActiveInventoryItem.pickupObj != null)
        {
            switch (ActiveInventoryItem.pickupObj)
            {
                case WeaponSO weaponObj:
                    equippedWeapon = weaponObj;
                    break;
                case ItemSO itemObj:
                    equippedItem = itemObj;
                    break;
            }
            Debug.Log($"Equipped {ActiveInventoryItem.name}");
        }
    }

    private void CheckNum(int i)
    {
        GameMenuManager.Instance.ClearSelectedItem();
        ActiveInventoryItem = null;
        if (inventoryList.ElementAtOrDefault(i) != null)
        {
            ActiveInventoryItem = inventoryList[i];
            GameMenuManager.Instance.SelectItem(i);
        }
    }

    private void HandleLevelUp()
    {
        curExperience -= expToLevel;
        curLevel += 1;
        expToLevel += 100;
        GameMenuManager.Instance.LevelDisplay.SetLevel(curLevel);
        GameMenuManager.Instance.SkillPrompt.AddToSkillQueue(this);
    }

}
