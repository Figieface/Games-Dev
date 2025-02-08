using System.Collections.Generic;
using UnityEngine;

public class EntitySummoner : MonoBehaviour
{
    public static List<Enemy> EnemiesInGame;
    public static Dictionary<int, GameObject> EnemyPrefabs;
    public static Dictionary<int, Queue<Enemy>> EnemyObjectPools;

    private static bool _isInitialised;

    private Vector3 testVec;

    public static void Init()
    {
        if (!_isInitialised)
        {
            EnemyPrefabs = new Dictionary<int, GameObject>();//Dictionaries are just pair srucs basically
            EnemyObjectPools = new Dictionary<int, Queue<Enemy>>();
            EnemiesInGame = new List<Enemy>();

            EnemySummonData[] Enemies = Resources.LoadAll<EnemySummonData>("Enemies");

            foreach (EnemySummonData enemy in Enemies)
            {
                EnemyPrefabs.Add(enemy.EnemyId, enemy.EnemyPrefab);
                EnemyObjectPools.Add(enemy.EnemyId, new Queue<Enemy>());
            }
            _isInitialised = true;
        }
        else
        {
            Debug.Log("ENTITY SUMMONER: THIS CLASS IS ALREADY INITIALSED");
        }
    }

    public static Enemy SummonEnemy(int EnemyID)
    {
        Enemy SummonedEnemy = null;
        var testVec = new Vector3(5, 2, 7);

        if (EnemyPrefabs.ContainsKey(EnemyID))
        {
            Queue<Enemy> ReferencedQueue = EnemyObjectPools[EnemyID];
            
            if(ReferencedQueue.Count > 0) //allows us to reuse enemies via object pooling
            {
                //Dequeue enemy and init

                SummonedEnemy = ReferencedQueue.Dequeue();
                SummonedEnemy.transform.position = testVec;
                SummonedEnemy.gameObject.SetActive(true);//reactivating our recycled enemy
                SummonedEnemy.Init();
            }
            else
            {
                //Instantate new instance of enemy and init
                //Vector3.zero is the spawn poin will need to set this
                GameObject NewEnemy = Instantiate(EnemyPrefabs[EnemyID], testVec, Quaternion.identity); //creating new enemy as none to reuse
                SummonedEnemy = NewEnemy.GetComponent<Enemy>();
                SummonedEnemy.Init();
            }
        }
        else
        {
            Debug.Log($"ENTITYSUMMONER: ENEMY WITH ID {EnemyID} DOES NOT EXIST"); //if enemy ID doesnt exist
            return null;
        }
        EnemiesInGame.Add(SummonedEnemy);
        SummonedEnemy.ID = EnemyID; //summoned enemy ID is the one we passed ino method
        return SummonedEnemy;
    }

    public static void RemoveEnemy(Enemy EnemyToRemove)
    {
        EnemyObjectPools[EnemyToRemove.ID].Enqueue(EnemyToRemove); //puts removed enemy back into queue for reuse
        EnemyToRemove.gameObject.SetActive(false); //deactivating the enemy
        EnemiesInGame.Remove(EnemyToRemove); //removes enemy from enemies in game list
    }
}
