using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Interactable : MonoBehaviour
{
    public string message;
    private Camera MainCamera;
    private bool IsInteractModalActive;
    private bool IsSpeechModalActive;
    private UIDocManagerOld docManagerOld;
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

        docManagerOld = UIDocManagerOld.Instance;
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
        docManagerOld.Modals.MoveInteractModal(screenPos);
    }
    private void MoveSpeechModal()
    {
        Vector2 screenPos = MainCamera.WorldToViewportPoint(transform.position);
        docManagerOld.Modals.MoveSpeechModal(screenPos);
    }

    public void Activate(PlayerControllerOld player)
    {
        Debug.Log(modalType);
        switch (modalType)
        {
            case InteractableType.Message when IsSpeechModalActive == false:
                docManagerOld.Modals.ToggleSpeechModal();
                docManagerOld.Modals.ChangeSpeechModalText(speechModalMessage);
                IsSpeechModalActive = true;
                StartCoroutine(DisableSpeechModal());
                break;
            case InteractableType.Shop:
                docManagerOld.ShopMenu.ToggleShopWindow();
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
        docManagerOld.Modals.ToggleSpeechModal();
        IsSpeechModalActive = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<PlayerControllerOld>();
        if (player == null)
        {
            return;
        }
        docManagerOld.Modals.ToggleInteractModal();
        docManagerOld.Modals.ChangeInteractModalText(message);
        IsInteractModalActive = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerControllerOld>();
        if (player == null)
        {
            return;
        }
        IsInteractModalActive = false;
        docManagerOld.Modals.ToggleInteractModal();
        switch (modalType)
        {
            case InteractableType.Message:
                break;
            case InteractableType.Shop:
                docManagerOld.ShopMenu.DisableShopWindow();
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