using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AppendageController : MonoBehaviour
{
    public int appendageHp;
    public BossController bossController;
    public List<AttackController> attacks;

    public bool detach;
    public bool affectsBossHp;

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

        var children = transform.GetComponentsInChildren<Transform>()
            .Where(t => t.gameObject.GetComponent<HitboxController>() == null);
        foreach (Transform child in children)
        {
            child.tag = "Uncollidable";
        }

        var hitboxes = transform.GetComponentsInChildren<HitboxController>();
        foreach (HitboxController hitbox in hitboxes)
        {
            hitbox.gameObject.layer = LayerMask.NameToLayer("EnemyHitbox");
            hitbox.bossController = bossController;
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
