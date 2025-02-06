using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    public bool endLoop;

    private static Queue<int> _enemyIDsToSummon;

    private void Start()
    {
        _enemyIDsToSummon = new Queue<int>();
        EntitySummoner.Init();

        StartCoroutine(GameLoop());
        InvokeRepeating("SummonTest", 0f, 1f);
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

            //Tick towers

            //Apply effects

            //Damage enemies

            //Remove enemies

            //Removes towers

            yield return null;
        }
    }

    public static void EnqueueEnemyIDToSummon(int ID)
    {
        _enemyIDsToSummon.Enqueue(ID);
    }
}
