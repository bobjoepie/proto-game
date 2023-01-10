using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBehaviorController : MonoBehaviour
{
    public AgentBehaviorType currentBehavior;
    public BehaviorSO idleBehavior;
    public BehaviorSO activeBehavior;
    public CircleCollider2D behaviorRadius;
    public PlayerController playerTarget;
    public float speed;

    private void Start()
    {

    }

    private void Update()
    {
        switch (currentBehavior)
        {
            case AgentBehaviorType.Idle:
                break;
            case AgentBehaviorType.Active:
                transform.position = Vector2.MoveTowards(transform.position, playerTarget.transform.position, speed * Time.deltaTime);
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent<PlayerController>(out var player)) return;
        playerTarget = player;
        currentBehavior = AgentBehaviorType.Active;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<PlayerController>(out var player)) return;
        playerTarget = player;
        currentBehavior = AgentBehaviorType.Active;
    }
}

public enum AgentBehaviorType
{
    None,
    Idle,
    Active
} 
