using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    public List<BossController> bosses;
    public List<EnemyController> enemies;

    private void OnEnable()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
