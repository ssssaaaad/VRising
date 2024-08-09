using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    public List<Enemy> groupEnemys = new List<Enemy>();
    public List<Transform> enemyOrigin = new List<Transform>();

    private void Awake()
    {
        
    }

    private void initGroup()
    {
        for (int i = 0; i < groupEnemys.Count; i++)
        {
            groupEnemys[i].SetOrigin(enemyOrigin[i]);
            groupEnemys[i].InitEnemy();
        }
    }

    public void Death(Enemy enemy)
    {
        int index = groupEnemys.IndexOf(enemy);
        groupEnemys.RemoveAt(index);
        enemyOrigin.RemoveAt(index);
    }

    public void SetTarget(Transform target)
    {
        for (int i = 0; i < groupEnemys.Count; i++)
        {
            groupEnemys[i].SetTarget(target);
        }
    }
}
