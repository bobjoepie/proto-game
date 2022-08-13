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
        var player = col.GetComponent<PlayerController>();
        if (player != null)
        {
            playerTarget = player;
            currentBehavior = AgentBehaviorType.Active;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            playerTarget = null;
            currentBehavior = AgentBehaviorType.Idle;
        } 
    }
}

public enum AgentBehaviorType
{
    None,
    Idle,
    Active
} 
