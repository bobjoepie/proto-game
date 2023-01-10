using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppendageController : MonoBehaviour
{
    public BossController bossController;
    public List<AttackController> attacks;

    public bool detach;

    private void Awake()
    {
        attacks = new List<AttackController>();
    }

    void Start()
    {
        bossController = transform.root.GetComponent<BossController>();
        if (bossController != null)
        {
            bossController.Register(this);
        }

        foreach (Transform child in transform)
        {
            child.tag = "Uncollidable";
        }
        
        if (detach)
        {
            this.transform.parent = null;
            var rigidbody = this.GetOrAddComponent<Rigidbody2D>();
            rigidbody.gravityScale = 0;
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void OnDestroy()
    {
        bossController.Unregister(this);
    }

    public void PerformAttacks(Vector2 targetPos)
    {
        foreach (var attack in attacks)
        {
            attack.Attack(targetPos).Forget();
        }
    }

    public void Register(AttackController attackController)
    {
        attacks.Add(attackController);
    }

    public void Unregister(AttackController attackController)
    {
        attacks.Remove(attackController);
    }
}
