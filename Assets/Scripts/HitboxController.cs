using UnityEngine;

public class HitboxController : MonoBehaviour
{
    public BossController bossController;

    public void TakeDamage(int damage)
    {
        bossController.TakeDamage(damage);
    }
}
