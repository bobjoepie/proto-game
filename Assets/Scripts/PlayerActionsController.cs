using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerActionsController : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    private Camera MainCamera;
    private bool CanAttack;
    private bool CanUseItem;
    private bool CanInteract;

    public Coroutine usingWeapon;
    public Coroutine usingItem;
    public Coroutine isInteracting;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        CanAttack = true;
        CanUseItem = true;
        CanInteract = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && player.equippedWeapon is not null)
        {
            PerformAttack();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && player.equippedItem is not null)
        {
            TryUseItem();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void PerformAttack()
    {
        if (CanAttack)
        {
            usingWeapon = StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        CanAttack = false;

        Vector2 playerPos = transform.position;
        Vector2 mousePos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        var dir = playerPos - mousePos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var rotToMouse = Quaternion.Euler(0f, 0f, angle);
        var weaponObj = player.equippedWeapon;

        dynamic weaponParts = WeaponSO.ConvertWeaponToParts(weaponObj);

        for (int i = 0; i < weaponObj.amount; i++)
        {
            WeaponSO.InstantiateWeaponParts(weaponParts, transform.position, rotToMouse);

            if (weaponObj.amountBurstTime > 0f)
            {
                yield return new WaitForSeconds(weaponObj.amountBurstTime);
            }
        }
        yield return new WaitForSeconds(weaponObj.cooldown * player.cooldownRate);
        CanAttack = true;
        usingWeapon = null;
    }

    private void TryUseItem()
    {
        if (CanUseItem)
        {
            usingItem = StartCoroutine(UseItem());
        }
    }

    IEnumerator UseItem()
    {
        CanUseItem = false;
        var itemObj = player.equippedItem;
        Debug.Log($"Used Item {itemObj.name}!");
        var colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (var col in colliders)
        {
            var checkItemComp = col.GetComponent<ItemChecker>();
            var item = checkItemComp?.itemToCheck;
            if (item is not null && item == player.equippedItem)
            {
                checkItemComp.MatchedItem(player);
                break;
            }
            
        }
        yield return new WaitForSeconds(itemObj.cooldown);
        CanUseItem = true;
        usingItem = null;
    }

    private void TryInteract()
    {
        if (CanInteract)
        {
            Debug.Log("E");
            isInteracting = StartCoroutine(Interact());
        }
    }

    IEnumerator Interact()
    {
        CanInteract = false;
        List<Collider2D> contacts = new List<Collider2D>();
        player.playerCollider.GetContacts(contacts);

        foreach (var contact in contacts)
        {
            if (!contact.isTrigger) continue;
            var interactable = contact.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Activate(player);
                yield return new WaitForSeconds(1f);
            }
        }
        CanInteract = true;
        isInteracting = null;
    }
}
