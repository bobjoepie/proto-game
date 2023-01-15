using DG.Tweening;
using UnityEngine;

public class LeashController : MonoBehaviour
{
    public GameObject leashAnchorObj;
    public GameObject lookTowardsObj;

    public bool rotateTowardsAnchor;
    [Range(0, 20)] public float minLeashRange = 0;
    [Range(0, 20)] public float maxLeashRange = 20;

    private bool isMoving = false;

    // Update is called once per frame
    void Update()
    {
        if (isMoving || leashAnchorObj == null) return;
        var curLeashDistance = Vector2.Distance(leashAnchorObj.transform.position, transform.position);
        if (curLeashDistance < minLeashRange)
        {
            isMoving = true;
            var dir = leashAnchorObj.transform.position - transform.position;
            var dist = minLeashRange - curLeashDistance;
            var targetPos = gameObject.transform.position - (dir.normalized * dist);
            Sequence s = DOTween.Sequence();
            s.Append(transform.DOMove(targetPos, 0.005f));
            if (lookTowardsObj != null)
            {
                s.Join(transform.DORotateQuaternion(transform.position.AngleTowards2D(lookTowardsObj.transform.position), 0.005f));
            }
            else if (rotateTowardsAnchor)
            {
                s.Join(transform.DORotateQuaternion(transform.position.AngleTowards2D(leashAnchorObj.transform.position), 0.005f));
            }
            s.Play()
                .OnComplete(() =>
                {
                    isMoving = false;
                });
        }
        else if (curLeashDistance > maxLeashRange)
        {
            isMoving = true;
            var dir = transform.position - leashAnchorObj.transform.position;
            var dist = curLeashDistance - maxLeashRange;
            var targetPos = gameObject.transform.position - (dir.normalized * dist);
            Sequence s = DOTween.Sequence();
            s.Append(transform.DOMove(targetPos, 0.005f));
            if (lookTowardsObj != null)
            {
                s.Join(transform.DORotateQuaternion(transform.position.AngleTowards2D(lookTowardsObj.transform.position),0.005f));
            }
            else if (rotateTowardsAnchor)
            {
                s.Join(transform.DORotateQuaternion(transform.position.AngleTowards2D(leashAnchorObj.transform.position), 0.005f));
            }
            s.Play()
                .OnComplete(() =>
                {
                    isMoving = false;
                });
        }
    }
}
