using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ActionRadiusController : MonoBehaviour
{
    public LayerMask layer;
    public List<Transform> targets;
    public bool isManagingTargets = true;
    public bool autoSortEnabled = true;
    public int autoSortFrequency = 5;

    private CancellationTokenSource cancellationToken;

    private void Awake()
    {
        gameObject.layer = layer.ToLayer();
        if (autoSortEnabled)
        {
            StartAutoSortTargets();
        }
    }
    private void OnEnable()
    {
        if (transform.root.TryGetComponent<EntityController>(out var entity))
        {
            entity.Register(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (isManagingTargets && col.transform.root.TryGetComponent<EntityController>(out var entity))
        {
            AddTarget(entity.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isManagingTargets && other.transform.root.TryGetComponent<EntityController>(out var entity))
        {
            RemoveTarget(entity.transform);
        }
    }

    private void OnDisable()
    {
        if (transform.root.TryGetComponent<EntityController>(out var entity))
        {
            entity.Unregister(this);
        }
    }

    public void AddTarget(Transform t)
    {
        targets.Add(t);
    }

    public void RemoveTarget(Transform t)
    {
        targets.Remove(t);
    }

    public void FillTargets()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D()
        {
            layerMask = layer
        };
        this.GetComponent<CircleCollider2D>().OverlapCollider(contactFilter, colliders);
        targets = colliders.Select(c => c.transform).ToList();
    }

    public void ClearTargets()
    {
        targets.Clear();
    }

    public void SortTargets()
    {
        targets = targets.OrderBy(
            t => Vector2.Distance(transform.position, t.transform.position)
        ).ToList();
    }

    private async UniTask AutoSortTargets()
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (autoSortEnabled)
            {
                SortTargets();
            }
            await UniTask.Delay(TimeSpan.FromSeconds(autoSortFrequency));
        }
    }

    public void StartAutoSortTargets()
    {
        if (cancellationToken != null) return;
        cancellationToken = new CancellationTokenSource();
        AutoSortTargets().Forget();
    }

    public void CancelAutoSortTargets()
    {
        cancellationToken.Cancel();
        cancellationToken.Dispose();
        cancellationToken = null;
        autoSortEnabled = false;
    }

    public Transform GetClosestTarget()
    {
        return targets.FirstOrDefault();
    }

    public List<Transform> GetClosestTargets(int num)
    {
        return targets.GetRange(0, num);
    }
}
