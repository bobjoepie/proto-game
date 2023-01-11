using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
    [Range(0f, 10f)] public float movementSpeed = 5f;
    [Range(0f, 30f)] public float rotationSpeed = 5f;

    private bool isRotating = false;
    private Sequence movementSequence;

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

        if (!isRotating)
        {
            RotateTowardsTarget();
        }

        MoveTowardsTarget();
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

    private void RotateTowardsTarget()
    {
        isRotating = true;
        movementSequence = DOTween.Sequence();
        movementSequence.Append(transform.DORotateQuaternion(transform.position.AngleTowards2D(target.position), actionSpeed));
        movementSequence.Play().OnComplete(() => { isRotating = false; });
    }

    private void MoveTowardsTarget()
    {
        var curLeashDistance = Vector2.Distance(target.position, transform.position);
        if (curLeashDistance < minLeashRange)
        {
            var dir = target.position - transform.position;
            var dist = minLeashRange - curLeashDistance;
            // 1.05f mult for edge case where it can't move enough to reach the RotateAround, but stuck here
            var targetPos = gameObject.transform.position - (dir.normalized * (dist * 1.05f));
            transform.position = Vector2.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);
        }
        else if (curLeashDistance > maxLeashRange)
        {
            var dir = transform.position - target.position;
            var dist = curLeashDistance - maxLeashRange;
            var targetPos = gameObject.transform.position - (dir.normalized * (dist * 1.05f));
            transform.position = Vector2.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);
        }
        else
        {
            if (Time.time % 10 > 5)
            {
                transform.RotateAround(target.position, new Vector3(0, 0, 1), Time.deltaTime * rotationSpeed);
            }
            else
            {
                transform.RotateAround(target.position, new Vector3(0, 0, -1), Time.deltaTime * rotationSpeed);
            }
        }
    }

    private void MoveTowardsTarget2()
    {
        var curLeashDistance = Vector2.Distance(target.position, transform.position);
        if (curLeashDistance < minLeashRange)
        {
            isRotating = true;
            var dir = target.position - transform.position;
            var dist = minLeashRange - curLeashDistance;
            var targetPos = gameObject.transform.position - (dir.normalized * dist);
            movementSequence = DOTween.Sequence();
            //movementSequence.Append(transform.DOMove(targetPos, actionSpeed));
            movementSequence.Join(transform.DORotateQuaternion(transform.position.AngleTowards2D(target.position), actionSpeed));
            movementSequence.Play().OnComplete(() => { isRotating = false; });
        } 
        else if (curLeashDistance > maxLeashRange)
        {
            isRotating = true;
            var dir = transform.position - target.position;
            var dist = curLeashDistance - maxLeashRange;
            var targetPos = gameObject.transform.position - (dir.normalized * dist);
            movementSequence = DOTween.Sequence();
            //movementSequence.Append(transform.DOMove(targetPos, actionSpeed));
            movementSequence.Join(transform.DORotateQuaternion(transform.position.AngleTowards2D(target.position), actionSpeed));
            movementSequence.Play().OnComplete(() => { isRotating = false; });
        }
        else
        {
            isRotating = true;
            movementSequence = DOTween.Sequence();
            movementSequence.Append(transform.DORotateQuaternion(transform.position.AngleTowards2D(target.position), actionSpeed));
            movementSequence.Play().OnComplete(() => { isRotating = false; });
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
            DestroyAll();
        }
    }

    private void DestroyAll()
    {
        foreach (var appendage in appendages)
        {
            Destroy(appendage.gameObject);
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.Unregister(this);
        }
    }
}
