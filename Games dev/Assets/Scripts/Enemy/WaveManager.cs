using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    [SerializeField] Pathfinding pathfinding;
    [SerializeField] WaveSpawner waveSpawner;

    [SerializeField] private bool addWave;
    [SerializeField] public List<Vector3Int> myWaypoints; //can turn to private once method made to send these to enemies
    Vector3Int mySpawnpoint, myDestination;


    private void Start()
    {
        myDestination = new Vector3Int(0, 0, 0);
        mySpawnpoint = new Vector3Int(10, 0, 10);
    }
    private void Update()
    {
        if (addWave == true)
        {
            SpawnAWave(0,10,3,mySpawnpoint);
            addWave = false;
        }
    }
    //NOTE THAT currently pathfinder has a bool to generate the grid to start
    private List<Vector3Int> GetWaypoints(Vector3Int spawnPoint, Vector3Int endPoint) //method to reduce amount of waypoints, so enemies can just head to corners of the path + end
    {
        //Debug.Log("GetWaypoints has ran");
        List<Vector3Int> waypoints = new();
        List<Vector3Int> path = pathfinding.GenGridandPath(spawnPoint, endPoint); //returns vec list of positions to get to 0,0,0 using a* 
        for (int i = path.Count - 2; i > 0; i--) //i starts at 1 (to skip spawnpoint) as you will always go straight from spawn no matter which way it paths
        {
            /*if (i == path.Count - 1)
            { //if were on the last vector (destination) as absvecdiff would try and compare a null
                waypoints.Add(path[i]);
                Debug.Log("hello");
            }*/
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

    private void SpawnAWave(int ID, int enemyCount, float spawnsPerSec, Vector3Int spawn) //input the ID so it can get the prefab from enemyDB, enemy count to set the count and can choose spawn locations
    {
        List<Vector3Int> wavePath = GetWaypoints(spawn, myDestination); 
        waveSpawner.WaveFromManager(ID, enemyCount, spawnsPerSec, spawn, wavePath);
    }
}
