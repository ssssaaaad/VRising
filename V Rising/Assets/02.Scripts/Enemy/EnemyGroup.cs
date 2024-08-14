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
    public List<EnemyData> groupEnemyDatas = new List<EnemyData>();

    private void Awake()
    {
        initGroup();
    }

    private void initGroup()
    {
        for (int i = 0; i < groupEnemyDatas.Count; i++)
        {
            groupEnemyDatas[i].enemy.SetOrigin(groupEnemyDatas[i].enemyOrigin);
            groupEnemyDatas[i].enemy.InitEnemy();
            groupEnemyDatas[i].enemy.SetEnemyGroup(this);
        }
    }

    public void Death(Enemy enemy)
    {
        for (int i = 0; i < groupEnemyDatas.Count; i++)
        {
            if (groupEnemyDatas[i].enemy == enemy)
            {
                groupEnemyDatas.RemoveAt(i);
            }
        }
    }

    public void SetTarget(Transform target)
    {
        for (int i = 0; i < groupEnemyDatas.Count; i++)
        {
            if (groupEnemyDatas[i].enemy.target == null)
                groupEnemyDatas[i].enemy.SetTarget(target);
        }
    }
}
