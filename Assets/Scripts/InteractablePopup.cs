using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePopup : MonoBehaviour
{
    public string message;
    private Camera MainCamera;
    private bool IsInteractModalActive;
    private bool IsSpeechModalActive;
    public UIDocManager docManager;
    public InteractableType modalType;
    public string speechModalMessage;
    public float fadeTime;

    private void Start()
    {
        MainCamera = Camera.main;
        IsInteractModalActive = false;
        IsSpeechModalActive = false;
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

    public void Activate()
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
        docManager.Modals.ToggleInteractModal();
        docManager.Modals.ChangeInteractModalText(message);
        IsInteractModalActive = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IsInteractModalActive = false;
        docManager.Modals.ToggleInteractModal();
        switch (modalType)
        {
            case InteractableType.Message:
                break;
            case InteractableType.Shop:
                docManager.ShopMenu.DisableShopWindow();
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
}