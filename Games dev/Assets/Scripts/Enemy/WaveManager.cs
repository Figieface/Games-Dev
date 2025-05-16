
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    [SerializeField] Pathfinding pathfinding;
    [SerializeField] WaveSpawner waveSpawner;

    [SerializeField] private bool addWave;
    [SerializeField] public List<Vector3Int> myWaypoints; //can turn to private once method made to send these to enemies
    Vector3Int waveDestination;
    private List<Vector3Int> spawnLocations = new List<Vector3Int>
    {
        new Vector3Int(-23, 0, 22),
        new Vector3Int(-23, 0, 0),//
        new Vector3Int(0, 0, 22),
        new Vector3Int(23, 0, 22),
        new Vector3Int(23, 0, 0),
        new Vector3Int(23, 0, -16),
        new Vector3Int(-23, 0, -16),
        new Vector3Int(0, 0, -16)
    };

    private int roundCounter;
    [SerializeField] List<List<WaveData>> rounds;
    [SerializeField] private List<EnemyData> levelEnemies;


    [SerializeField] private EnemyDBSO database;
    [SerializeField] private GameObject endLevelUI;
    [SerializeField] public TextMeshProUGUI roundCounterUI;

    private bool victorySound = false;

    private void Start()
    {
        AudioManager.gamemusicSound();
        //Time.timeScale = 5f;
        for (int i = 0; i < 3; i++)
        {
            int randomEnemy = UnityEngine.Random.Range(1,6);
            levelEnemies.Add(database.enemyData.Find(iD => iD.ID == randomEnemy));//adding enemy
        }

        roundCounter = 0;
        waveDestination = new Vector3Int(0, 0, 0);

        GenerateRounds();
    }

    private void Update()
    {
        if (roundCounter >= 10 && !waveSpawner.allowSpawning)
        {
            EndLevel();
        }
    }

    public void AddNextRoundWaves() //onclick for the next round button
    {
        /*for (int i = 0; i < rounds.Count; i++)
        {
            Debug.Log($"List {i} has {rounds[i].Count} elements.");
        }*/
        for (int i = 0; i < rounds[roundCounter].Count; i++) //passing along all waves in a round
        {
            WaveData currentWave = rounds[roundCounter][i];
            SpawnAWave(currentWave.EnemyID, currentWave.EnemyAmount, currentWave.SpawnsPerSec, currentWave.SpawnPoint, currentWave.WaveGroup);
            //Debug.Log($"enemyID:{currentWave.EnemyID},num:{currentWave.EnemyAmount},spawnrate:{currentWave.SpawnsPerSec},spawnpoint:{currentWave.SpawnPoint},wavegroup:{currentWave.WaveGroup}");
        }
        AllowWaves();
        roundCounter++; //puts the round counter up one
        roundCounterUI.text = $"Round: {roundCounter}/10";
    }

    private void EndLevel()
    {
        endLevelUI.SetActive(true);
        if (victorySound == false)
        {
            AudioManager.wonSound();
            victorySound = true;
        }
    }

    private void GenerateRounds()
    {
        int enemyNum = 0;
        rounds = new List<List<WaveData>>();
        for (int roundNumber = 1; roundNumber <= 10; roundNumber++) //10 rounds per level
        {
            //Debug.Log(roundNumber);
            rounds.Add(new List<WaveData>()); //add a row
            int budget = GetRoundDifficultyBudget(roundNumber);
            while (budget > 0) //while the budget for the round 
            {
                List<EnemyData> validEnemyChoice = levelEnemies.FindAll(enemy => enemy.Cost <= budget); //currently just using ID as cost, will probably change this later
                if (validEnemyChoice.Count == 0) break; //if somehow unable to spawn something
                //selecting the attributes for each wave
                EnemyData selectedEnemy = validEnemyChoice[UnityEngine.Random.Range(0,validEnemyChoice.Count)];
                if (budget - (selectedEnemy.Cost * 20) > 0)
                {
                    enemyNum = UnityEngine.Random.Range(1, 20 + 1);
                }
                else
                {
                    enemyNum = UnityEngine.Random.Range(1, (budget / selectedEnemy.Cost) + 1);
                }
                
                Vector3Int spawningPoint = spawnLocations[UnityEngine.Random.Range(0,spawnLocations.Count)];
                int waveGroupNum = (rounds[roundNumber - 1].Count / 4);

                //Debug.Log(enemyNum);
                WaveData waveInstance = new WaveData(selectedEnemy.ID, enemyNum, 1, spawningPoint, waveGroupNum);

                rounds[roundNumber - 1].Add(waveInstance);//added to the row/round

                budget -= selectedEnemy.Cost * enemyNum;
            }
        }
    }


    private List<Vector3Int> GetWaypoints(Vector3Int spawnPoint, Vector3Int endPoint) //method to reduce amount of waypoints, so enemies can just head to corners of the path + end
    {
        List<Vector3Int> waypoints = new();
        List<Vector3Int> path = pathfinding.GenGridandPath(spawnPoint, endPoint); //returns vec list of positions to get to 0,0,0 using a* 
        for (int i = path.Count - 2; i > 0; i--) //i starts at 1 (to skip spawnpoint) as you will always go straight from spawn no matter which way it paths
        {
            if (AbsVecDiff(path[i], path[i - 1]) == AbsVecDiff(path[i], path[i + 1])) //if the vec diff before is the same as the one after
                continue; //then we can skip as we dont need to travel to it
            else
                waypoints.Add(path[i]);//if not it means it is a corner and we add it to waypoints
        }
        waypoints.Add(endPoint);
        return waypoints;
    }

    private Vector3Int AbsVecDiff(Vector3Int v1, Vector3Int v2)
    {
        Vector3Int v3 = v1 - v2;

        return new Vector3Int(
            Mathf.Abs(v3.x),
            Mathf.Abs(v3.y),
            Mathf.Abs(v3.z)
        );
    }

    public void SpawnAWave(int ID, int enemyCount, float spawnsPerSec, Vector3Int spawn, int waveNum) //input the ID so it can get the prefab from enemyDB, enemy count to set the count and can choose spawn locations
    {
        List<Vector3Int> wavePath = GetWaypoints(spawn, waveDestination); 
        waveSpawner.WaveFromManager(ID, enemyCount, spawnsPerSec, spawn, wavePath, waveNum);
    }

    public void AllowWaves() //allow the waves to spawn after 
    {
        waveSpawner.allowSpawning = true;
    }

    private int GetRoundDifficultyBudget(int round)
    {
        return Mathf.RoundToInt(DifficultyManager.gameDifficulty + (round * 4));
    }
}

public class WaveData
{
    public int EnemyID;
    public int EnemyAmount;
    public int SpawnsPerSec;
    public Vector3Int SpawnPoint;
    public int WaveGroup;

    public WaveData(int enemyID, int enemyAmount, int spawnsPerSec, Vector3Int spawnPoint, int waveGroup)
    {
        EnemyID = enemyID;
        EnemyAmount = enemyAmount;
        SpawnsPerSec = spawnsPerSec;
        SpawnPoint = spawnPoint;
        WaveGroup = waveGroup;
    }
}