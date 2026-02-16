using NUnit.Framework;
using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    private List<AIDestinationSetter> enemies = new List<AIDestinationSetter>();

    private void Awake()
    {
        instance = this;
    }

    public void Register(AIDestinationSetter enemy)
    {
        if (!enemies.Contains(enemy))
            enemies.Add(enemy);
    }

    public void Unregister(AIDestinationSetter enemy)
    {
        enemies.Remove(enemy);
    }

    public void DisableAllEnemies()
    {
        foreach (AIDestinationSetter e in enemies)
        {
            e.enabled = false;
        }
    }

    public void EnableAllEnemies()
    {
        foreach (AIDestinationSetter e in enemies)
        {
            e.enabled = true;
        }
    }



}
