using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : CreateableObjects
{
    public override string ObjectTypeName => "enemy";

    public override Transform Spawn(Vector3 position)
    {
        //create the new enemy
        Transform newEnemy = Object.Instantiate(Resources.Load("Prefab/Enemy Prefab", typeof(GameObject)) as GameObject, position, Quaternion.identity).transform;

        //set the start and end of it's path
        EnemyController enemyController = newEnemy.GetComponentInChildren<EnemyController>();
        enemyController.pathStart = position;
        enemyController.pathEnd = position + new Vector3(0.0f, 0.0f, 5.0f);

        //and return it
        return newEnemy;
    }
}
