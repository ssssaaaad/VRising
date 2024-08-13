using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public Enemy enemy;
    public Transform enemyOrigin;
    public Transform[] patrolPoint;
}

public class EnemyGroup : MonoBehaviour
{
    public List<EnemyData> groupEnemys = new List<EnemyData>();

    private void Awake()
    {
        initGroup();
    }

    private void initGroup()
    {
        for (int i = 0; i < groupEnemys.Count; i++)
        {
            groupEnemys[i].SetOrigin(enemyOrigin[i]);
            groupEnemys[i].InitEnemy();
            groupEnemys[i].SetEnemyGroup(this);
        }
    }

    public void Death(Enemy enemy)
    {
        if (groupEnemys.Contains(enemy))
        {
            int index = groupEnemys.IndexOf(enemy);
            groupEnemys.RemoveAt(index);
            enemyOrigin.RemoveAt(index);
        }
    }

    public void SetTarget(Transform target)
    {
        for (int i = 0; i < groupEnemys.Count; i++)
        {
            if (groupEnemys[i].target == null)
                groupEnemys[i].SetTarget(target);
        }
    }
}
