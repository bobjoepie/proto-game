using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ManagerMaster : MonoBehaviour
{
    [FormerlySerializedAs("uiDocManager2")] public UIDocManager uiDocManager;
    public InputManager inputManager;
    public EnemyManager enemyManager;
    
    private void Awake()
    {
    }
}
