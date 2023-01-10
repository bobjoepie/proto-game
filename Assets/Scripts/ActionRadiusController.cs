using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRadiusController : MonoBehaviour
{
    public BossController bossController;
    private void Start()
    {
        bossController = transform.root.GetComponent<BossController>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent<PlayerController>(out var player))
        {
            bossController.AssignTarget(player.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            bossController.ClearTarget();
        }
    }
}
