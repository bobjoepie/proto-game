using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    public List<BossController> bosses;
    public List<EnemyController> enemies;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
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
                break;
        }
    }
}
