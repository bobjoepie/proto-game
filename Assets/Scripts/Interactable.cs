using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Interactable : MonoBehaviour
{
    public string message;
    private Camera MainCamera;
    private bool IsInteractModalActive;
    private bool IsSpeechModalActive;
    private UIDocManager docManager;
    public InteractableType modalType;
    public string speechModalMessage;
    public float fadeTime;
    public PickupSO pickupSO;
    public int quantity;

    private void Start()
    {
        MainCamera = Camera.main;
        IsInteractModalActive = false;
        IsSpeechModalActive = false;

        docManager = UIDocManager.Instance;
    }

    private void Update()
    {
        if (IsInteractModalActive)
        {
            MoveInteractModal();
        }

        if (IsSpeechModalActive)
        {
            MoveSpeechModal();
        }
    }

    private void MoveInteractModal()
    {
        Vector2 screenPos = MainCamera.WorldToViewportPoint(transform.position);
        docManager.Modals.MoveInteractModal(screenPos);
    }
    private void MoveSpeechModal()
    {
        Vector2 screenPos = MainCamera.WorldToViewportPoint(transform.position);
        docManager.Modals.MoveSpeechModal(screenPos);
    }

    public void Activate(PlayerController player)
    {
        switch (modalType)
        {
            case InteractableType.Message when IsSpeechModalActive == false:
                docManager.Modals.ToggleSpeechModal();
                docManager.Modals.ChangeSpeechModalText(speechModalMessage);
                IsSpeechModalActive = true;
                StartCoroutine(DisableSpeechModal());
                break;
            case InteractableType.Shop:
                docManager.ShopMenu.ToggleShopWindow();
                break;
            case InteractableType.Pickup:
                player.Pickup2(this);
                break;
            default:
                break;
        }
    }

    IEnumerator DisableSpeechModal()
    {
        yield return new WaitForSeconds(fadeTime);
        docManager.Modals.ToggleSpeechModal();
        IsSpeechModalActive = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<PlayerController>();
        if (player == null)
        {
            return;
        }
        docManager.Modals.ToggleInteractModal();
        docManager.Modals.ChangeInteractModalText(message);
        IsInteractModalActive = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player == null)
        {
            return;
        }
        IsInteractModalActive = false;
        docManager.Modals.ToggleInteractModal();
        switch (modalType)
        {
            case InteractableType.Message:
                break;
            case InteractableType.Shop:
                docManager.ShopMenu.DisableShopWindow();
                break;
            case InteractableType.Pickup:
                break;
            default:
                break;
        }
    }
}

public enum InteractableType
{
    Message,
    Shop,
    Pickup,

}