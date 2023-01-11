using DG.Tweening;
using UnityEngine;

public class GridToolbarButton : MonoBehaviour
{
    public Color origColor;
    public Color hoverColor;
    public Color selectColor;

    public SpriteRenderer colorObj;
    public GridToolbarHandler toolbarHandler;

    public GridHandler gridHandler;
    public Camera mainCamera;
    public virtual void Awake()
    {
        colorObj = GetComponent<SpriteRenderer>();
        origColor = colorObj.color;
        mainCamera = Camera.main;
    }

    public virtual void OnMouseOver()
    {
        TransitionColor(hoverColor, 0.25f);
    }

    public virtual void OnMouseExit()
    {
        TransitionColor(origColor, 0.25f);
    }

    protected void TransitionColor(Color color, float duration = 0.25f)
    {
        DOTween.To(
            () => colorObj.color,
            x => colorObj.color = x,
            color,
            duration
        );
    }
}
