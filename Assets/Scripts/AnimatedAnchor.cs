using DG.Tweening;
using UnityEngine;

public class AnimatedAnchor : MonoBehaviour
{
    [Range(0f,3f)] public float distance = 1f;
    [Range(0f,5f)] public float duration = 2.0f;

    public Ease ease = Ease.InSine;
    public LoopType loopType = LoopType.Yoyo;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOLocalMoveY(distance, duration)
            .SetEase(ease)
            .SetLoops(-1, loopType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
