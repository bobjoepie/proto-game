using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    public List<BossController> bosses = new List<BossController>();
    public List<EnemyController> enemies = new List<EnemyController>();
    private UIDocManager2 uiDocManager2;

    private EnemyManager()
    {
        Instance = this;
    }

    private void Awake()
    {
        if (UIDocManager2.Instance != null)
        {
            uiDocManager2 = UIDocManager2.Instance;
        }
    }

    public void Register<T>(T entity)
    {
        switch (entity)
        {
            case EnemyController e:
                enemies.Add(e);
                break;
            case BossController e:
                bosses.Add(e);
                uiDocManager2.bossHealthBar?.Enable();
                uiDocManager2.bossHealthBar?.SetName(e.attributes.name);
                break;
        }
    }

    public void Unregister<T>(T entity)
    {
        switch (entity)
        {
            case EnemyController e:
                enemies.Remove(e);
                break;
            case BossController e:
                bosses.Remove(e);
                uiDocManager2.bossHealthBar?.Disable();
                break;
        }
    }
}
