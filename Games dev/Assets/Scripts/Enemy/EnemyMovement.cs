using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] WaveManager waveManager;

    private float moveSpeed = 2.0f;
    [SerializeField] Transform pivot;
    private float rotationSpeed = 12.0f;

    [SerializeField] private List<Vector3Int> enemyWaypoints;

    private Vector3Int endPoint;
    private int waypointIndex = 0;

    [SerializeField] bool startMove, getWaypoints;

    private void Update()
    {
        if (getWaypoints == true)
        {
            enemyWaypoints = waveManager.myWaypoints; //getting path from wavemanager
            endPoint = enemyWaypoints[enemyWaypoints.Count - 1]; 
            getWaypoints = false;
        }
        if (startMove == true)
        {
            Path();
        }
    }

    private void Path()
    {
        Vector3 targetPosition = (Vector3)enemyWaypoints[waypointIndex]; //casting target position to be vec3
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime); //move

        Vector3 dir = (targetPosition - transform.position).normalized; //direction to look in

        if (dir != Vector3.zero) //if it not looking in dir 
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            pivot.rotation = Quaternion.Slerp(pivot.rotation, targetRotation, rotationSpeed * Time.deltaTime); //rotate to target dir
        }

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f) //if within 0.2f of the target
        {
            if (waypointIndex >= enemyWaypoints.Count)
            {
                Destroy(gameObject);//destroys itself
                return;
                //will need to 'die' and do damage
            }

            waypointIndex++; //iterates

        }
    }
}
