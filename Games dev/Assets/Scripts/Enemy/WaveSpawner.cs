using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;


public class WaveSpawner : MonoBehaviour
{
    [System.Serializable] public class Wave //making it changeable in editor for now
    {
        public string name;
        public GameObject enemy; //prefab
        public int count;
        public float spawnRate;
        public Vector3Int spawnPoint;
        public List<Vector3Int> path;


        public Wave(string name, GameObject enemy, int count, float spawnRate, Vector3Int spawnPoint, List<Vector3Int> path) //constructor
        {
            this.name = name;
            this.enemy = enemy;
            this.count = count;
            this.spawnRate = spawnRate;
            this.spawnPoint = spawnPoint;
            this.path = path;
        }
    }

    [SerializeField] private EnemyDBSO enemyDatabase;

    public enum SpawnState {SPAWNING, WAITING, COUNTING};

    public List<Wave> waves;
    private int nextWave = 0;
    public float timeBetweenWaves = 2f; //between waves of individual spawns- i will call what is normally a wave a 'round' for clarity
    public float waveCountdown;

    public GameObject[] spawnPoints;

    [SerializeField] private float enemyCheckCountdown = 2.0f;

    private SpawnState state = SpawnState.COUNTING;

    [SerializeField] private bool allowSpawning;

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
                if (!EnemyIsAlive())
                {
                    //new round starts
                    WaveCompleted();
                    return;
                }
                else
                {
                    return; //no need to count for waves if enemies are alive so just return
                }
            }

            if (waveCountdown <= 0) //if wave count down is still going
            {
                if (state != SpawnState.SPAWNING)
                {
                    StartCoroutine(SpawnWave(waves[nextWave])); //spawn wave method
                }
            }
            else
            {
                waveCountdown -= Time.deltaTime; //else cotinue countdown
            }
        }
    }

    public void WaveFromManager(int ID, int enemyCount, float spawnsPerSec, Vector3Int spawn, List<Vector3Int> wavePath)
    {
        int selectedEnemy = enemyDatabase.enemyData.FindIndex(data => data.ID == ID);  //gets index in the DB of the enemy ID passed
        Wave newWave = new Wave( //creating new wave from the data from wavemanager
            enemyDatabase.enemyData[selectedEnemy].Name,
            enemyDatabase.enemyData[selectedEnemy].Prefab,
            enemyCount,
            spawnsPerSec,
            spawn,
            wavePath);
        waves.Add(newWave);
    }

    private void WaveCompleted()
    {
        Debug.Log("Wave completed");

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;
        if (nextWave+1 > waves.Count-1) //if no more waves
        {
            nextWave = 0;
            Debug.Log("All waves complete");
        }
        else
        {
            nextWave++;
        }
        
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

    IEnumerator SpawnWave(Wave wave)
    {
        Debug.Log("Spawning wave: "+wave.name);
        state = SpawnState.SPAWNING;

        for (int i = 0; i < wave.count; i++) //while theres still waves left
        {
            //Debug.Log(wave.path);
            SpawnEnemy(wave.enemy, wave.path, wave.spawnPoint);
            yield return new WaitForSeconds(1f/wave.spawnRate);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    private void SpawnEnemy(GameObject enemy, List<Vector3Int> path, Vector3Int spawn) //once enemy class created gameobjects will need to be chnaged to that
    {
        GameObject enemyInstance = Instantiate(enemy, spawn, transform.rotation);
        EnemyMovement moveScript = enemyInstance.GetComponent<EnemyMovement>();
        moveScript.enemyWaypoints = path;
        //Debug.Log(moveScript.enemyWaypoints[0]+","+ moveScript.enemyWaypoints[1]);//
    }
}
