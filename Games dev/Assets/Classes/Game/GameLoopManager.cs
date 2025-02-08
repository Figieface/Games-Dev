using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    public bool endLoop;

    private static Queue<Enemy> _enemiesToRemove;
    private static Queue<int> _enemyIDsToSummon;

    private void Start()
    {
        _enemyIDsToSummon = new Queue<int>();
        _enemiesToRemove = new Queue<Enemy>();
        EntitySummoner.Init();

        StartCoroutine(GameLoop());
        InvokeRepeating("SummonTest", 0f, 3f);
    }

    void SummonTest()
    {
        EnqueueEnemyIDToSummon(1);
    }

    IEnumerator GameLoop()
    {
        while (endLoop == false)
        {

            //Spawn enemies

            if (_enemyIDsToSummon.Count > 0)
            {
                for (int i = 0; i < _enemyIDsToSummon.Count; i++)
                {
                    EntitySummoner.SummonEnemy(_enemyIDsToSummon.Dequeue());
                }
            }

            //Spawn towers

            //Move enemies

            //Moving via navmesh in their script

            //Tick towers

            //Apply effects

            //Damage enemies

            //Remove enemies
            for (int i = 0; i < EntitySummoner.EnemiesInGame.Count; i++)
            {
                //Checks whether any enemies have reached the base and enqueues to remove if so
                if (EntitySummoner.EnemiesInGame[i].endReached == true)
                {
                    EnqueueEnemyToRemove(EntitySummoner.EnemiesInGame[i]);
                }
            }

            if (_enemiesToRemove.Count > 0)
            {
                for (int i = 0; i < _enemiesToRemove.Count; i++)
                {
                    EntitySummoner.RemoveEnemy(_enemiesToRemove.Dequeue());
                }
            }

            //Removes towers

            yield return null;
        }
    }

    public static void EnqueueEnemyIDToSummon(int ID)
    {
        _enemyIDsToSummon.Enqueue(ID);
    }

    public static void EnqueueEnemyToRemove(Enemy EnemyToRemove)
    {
        _enemiesToRemove.Enqueue(EnemyToRemove);
    }
}
