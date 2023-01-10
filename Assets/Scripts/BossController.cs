using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public int health;
    public List<AppendageController> appendages;
    public bool CanAttack = true;
    public Transform target;
    [Range(0f, 20f)] public float minLeashRange = 0f;
    [Range(0f, 20f)] public float maxLeashRange = 20;
    [Range(0f, 1f)] public float actionSpeed = 0.5f;

    private bool isMoving = false;

    private void Start()
    {
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.Register(this);
        }
    }

    public void Register<T>(T component)
    {
        switch (component)
        {
            case AppendageController ap:
                appendages.Add(ap);
                break;
        }
    }

    public void Unregister<T>(T component)
    {
        switch (component)
        {
            case AppendageController ap:
                appendages.Remove(ap);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        if (CanAttack)
        {
            CanAttack = false;
            TryAttack().Forget();
        }

        if (!isMoving)
        {
            MoveTowardsTarget();
        }
    }

    private async UniTaskVoid TryAttack()
    {
        await TryPerformAttacks();
        await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
        CanAttack = true;
    }

    private async UniTask TryPerformAttacks()
    {
        foreach (AppendageController appendage in appendages)
        {
            if (target == null) return;
            appendage.PerformAttacks(target.position);
            await UniTask.Delay(TimeSpan.FromSeconds(0.15f));
        }
    }

    private void MoveTowardsTarget()
    {
        var curLeashDistance = Vector2.Distance(target.position, transform.position);
        if (curLeashDistance < minLeashRange)
        {
            isMoving = true;
            var dir = target.position - transform.position;
            var dist = minLeashRange - curLeashDistance;
            var targetPos = gameObject.transform.position - (dir.normalized * dist);
            Sequence s = DOTween.Sequence();
            s.Append(transform.DOMove(targetPos, actionSpeed));
            s.Join(transform.DORotateQuaternion(transform.position.AngleTowards2D(target.position), actionSpeed));
            s.Play()
                .OnComplete(() =>
                {
                    isMoving = false;
                });
        } 
        else if (curLeashDistance > maxLeashRange)
        {
            isMoving = true;
            var dir = transform.position - target.position;
            var dist = curLeashDistance - maxLeashRange;
            var targetPos = gameObject.transform.position - (dir.normalized * dist);
            Sequence s = DOTween.Sequence();
            s.Append(transform.DOMove(targetPos, actionSpeed));
            s.Join(transform.DORotateQuaternion(transform.position.AngleTowards2D(target.position), actionSpeed));
            s.Play()
                .OnComplete(() =>
                {
                    isMoving = false;
                });
        }
        else
        {
            isMoving = true;
            Sequence s = DOTween.Sequence();
            s.Append(transform.DORotateQuaternion(transform.position.AngleTowards2D(target.position), actionSpeed));
            s.Play()
                .OnComplete(() =>
                {
                    isMoving = false;
                });
        }
    }

    public void AssignTarget(Transform t)
    {
        target = t;
    }

    public void ClearTarget()
    {
        target = null;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.Unregister(this);
        }
    }
}
