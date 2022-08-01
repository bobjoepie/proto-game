using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class InteractablePopup : MonoBehaviour
{
    public string message;
    public GM_Modals UIModals;
    public GM_ShopMenu ShopMenu;
    private Camera MainCamera;
    private bool IsModalActive;

    private void Start()
    {
        UIModals = UIDocManager.Instance.Modals;
        ShopMenu = UIDocManager.Instance.ShopMenu;
        MainCamera = Camera.main;
        IsModalActive = false;
    }

    private void Update()
    {
        if (IsModalActive)
        {
            MoveModal();
        }
    }

    private void MoveModal()
    {
        Vector2 screenPos = MainCamera.WorldToViewportPoint(transform.position);
        UIModals.MoveModal(screenPos);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        UIModals.ToggleModal();
        UIModals.ChangeText(message);
        IsModalActive = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IsModalActive = false;
        UIModals.ToggleModal();
        ShopMenu.DisableShopWindow();
    }
}
