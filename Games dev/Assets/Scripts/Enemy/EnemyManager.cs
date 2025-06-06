using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] WaveManager waveManager;
    [SerializeField] EnemyDBSO database;

    public float moveSpeed = 2.0f;
    [SerializeField] Transform pivot;
    private float rotationSpeed = 12.0f;

    [SerializeField] public List<Vector3Int> enemyWaypoints; //should be set by the spawn method in wavespawner

    public int maxHP;
    public int currentHP;
    public int reward;

    private Vector3Int endPoint;
    private int waypointIndex = 0;

    public int damage;//to be changed by the spawner

    [SerializeField] public GameObject bloodSplatter;
    [SerializeField] public Transform effectLocation;

    private void Awake()
    {
        //enemyWaypoints.Add(Vector3Int.FloorToInt(transform.position));
        waveManager = FindFirstObjectByType<WaveManager>();
        currentHP = maxHP;
    }

    private void Start()
    {
        endPoint = enemyWaypoints[enemyWaypoints.Count - 1];
    }

    private void Update()
    {
        if (currentHP <= 0)
        {
            Die();
        }
        Path();
    }

    private void Die()
    {
        StructureShop.currency += reward;
        ScoreManager.score += reward;
        GameObject blood = Instantiate(bloodSplatter, effectLocation.position, effectLocation.rotation * Quaternion.Euler(-90f,0f,0f));
        Destroy(blood,2f);
        Destroy(gameObject);
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

        if (Vector3.Distance(transform.position, targetPosition) < 0.05f) //if within 0.2f of the target
        {
            waypointIndex++; //iterates

            if (waypointIndex >= enemyWaypoints.Count)
            {
                Destroy(gameObject);//destroys itself
                Lives.DamagePlayer(damage);
                Debug.Log("Reached end and damaged player!");
                return;
                //will need to 'die' and do damage
            }
        }
    }
}
