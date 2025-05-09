
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaveSpawner : MonoBehaviour
{
    [System.Serializable] public class Wave //making it changeable in editor for now
    {
        public string name;
        public GameObject enemy; //prefab
        public int count, waveGroup, damage;
        public float spawnRate;
        public Vector3Int spawnPoint;
        public List<Vector3Int> path;

        public Wave(string name, GameObject enemy, int count, float spawnRate, Vector3Int spawnPoint, List<Vector3Int> path, int waveGroup, int damage) //constructor
        {
            this.name = name;
            this.enemy = enemy;
            this.count = count;
            this.spawnRate = spawnRate;
            this.spawnPoint = spawnPoint;
            this.path = path;
            this.waveGroup = waveGroup;
            this.damage = damage;
        }
    }

    [SerializeField] private EnemyDBSO enemyDatabase;
    [SerializeField] public StructureShop structureShop;

    public enum SpawnState {SPAWNING, WAITING, COUNTING, BETWEENROUNDS};

    public List<Wave> waves;
    private bool nextWaveGroupExists = true;
    public float timeBetweenWaves = 2f; //time between wave groups
    public float waveCountdown;
    private int currentWaveGroup = 0;


    public GameObject[] spawnPoints;

    [SerializeField] private float enemyCheckCountdown = 2.0f;

    private SpawnState state = SpawnState.COUNTING;

    [SerializeField] public bool allowSpawning;

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
    }
    private void Update()
    {
        if (allowSpawning) //just a bool to manually turn on and off this script for testing
        {
            if (state == SpawnState.WAITING)
            {
                if (!EnemyIsAlive()) //check if enemies alive every 2 seconds
                {
                    RoundCompleted();
                    return;
                }
                else
                {
                    return; //no need to count for waves if enemies are alive so just return
                }
            }

            if (waveCountdown <= 0 && nextWaveGroupExists == true) //if wave count down is still going
            {
                if (state != SpawnState.SPAWNING)
                {
                    
                    //StartCoroutine(SpawnWave(waves[nextWaveGroupExists])); //spawn wave method
                    StartCoroutine(SpawnWaveGroups());
                }
            }
            else
            {
                waveCountdown -= Time.deltaTime; //else continue countdown
            }
        }
    }

    public void WaveFromManager(int ID, int enemyCount, float spawnsPerSec, Vector3Int spawn, List<Vector3Int> wavePath, int waveGroup)
    {
        int selectedEnemy = enemyDatabase.enemyData.FindIndex(data => data.ID == ID);  //gets index in the DB of the enemy ID passed
        Wave newWave = new Wave( //creating new wave from the data from wavemanager
            enemyDatabase.enemyData[selectedEnemy].Name,
            enemyDatabase.enemyData[selectedEnemy].Prefab,
            enemyCount,
            spawnsPerSec,
            spawn,
            wavePath,
            waveGroup,
            enemyDatabase.enemyData[selectedEnemy].Damage);
        waves.Add(newWave);
    }

    private void RoundCompleted()
    {
        Debug.Log("Round completed");
        state = SpawnState.COUNTING;
        waves.Clear(); //clearing waves for next round
        currentWaveGroup = 0; //resetting currentWaveGroup for next round
        nextWaveGroupExists = true; //nextwavegroup will exist next round
        allowSpawning = false;
        StructureShop.currency += 120;
        structureShop.ToggleShopUI();
    }

    private bool EnemyIsAlive() //checking whether any enemies are alive
    {
        enemyCheckCountdown -= Time.deltaTime;
        if (enemyCheckCountdown <= 0f)
        {
            //Debug.Log("enemy bool is called");
            enemyCheckCountdown = 2f;
            //Debug.Log(GameObject.FindGameObjectWithTag("Enemy"));
            if (GameObject.FindGameObjectWithTag("Enemy") == null) return false; //checks for objects with enemy tag, if null then false

        }
        return true; //if there were enemys then bool is true
    }

    IEnumerator SpawnWaveGroups()
    {
        List<Wave> currentWaves = GetWaves(currentWaveGroup);

        if (currentWaves.Count == 0) //no waves found in group
        {
            yield break;
        }
        state = SpawnState.SPAWNING;
        List<Coroutine> wavesSpawning = new List<Coroutine>();

        foreach (Wave wave in currentWaves) //starts all wave spawn coroutines in group
        {
            Coroutine waveCoroutine = StartCoroutine(SpawnWave(wave));
            wavesSpawning.Add(waveCoroutine);
        }

        foreach (Coroutine waveCoroutine in wavesSpawning) //wait for enemies to finish spawning
        {
            yield return waveCoroutine;
        }

        currentWaveGroup++;
        waveCountdown = timeBetweenWaves;
        state = SpawnState.COUNTING;
        if (nextWaveGroupExists == false) //if theres no next wave we need to wait for enemies to die
        {
            state = SpawnState.WAITING; 
        }
    }

    private List<Wave> GetWaves(int waveGroupNum) //method to return all waves with a specific wavenum
    {
        List<Wave> groupWaveList = new();
        foreach (Wave wave in waves)
        {
            if (wave.waveGroup == waveGroupNum) //getting waves of a certain group number
            {
                groupWaveList.Add(wave);
            }
        }
        nextWaveGroupExists = waves.Exists(wave => wave.waveGroup == waveGroupNum + 1); //goes false if theres no next wave group- last wave is being spawned
        return groupWaveList;
    }

    IEnumerator SpawnWave(Wave wave)
    {
        Debug.Log("Spawning wave: "+wave.name+" x"+wave.count);
        for (int i = 0; i < wave.count; i++) //while theres still enemies to spawn
        {
            //Debug.Log(wave.path);
            SpawnEnemy(wave.enemy, wave.path, wave.spawnPoint, wave.damage);
            yield return new WaitForSeconds(1f/wave.spawnRate);
        }
    }

    private void SpawnEnemy(GameObject enemy, List<Vector3Int> path, Vector3Int spawn, int damage) //once enemy class created gameobjects will need to be chnaged to that
    {
        GameObject enemyInstance = Instantiate(enemy, spawn, transform.rotation);
        EnemyMovement moveScript = enemyInstance.GetComponent<EnemyMovement>();
        moveScript.damage = damage;
        moveScript.enemyWaypoints = path;
        //Debug.Log(moveScript.enemyWaypoints[0]+","+ moveScript.enemyWaypoints[1]);//
    }
}
