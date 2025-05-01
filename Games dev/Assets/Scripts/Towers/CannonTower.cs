using System;
using UnityEngine;

public class CannonTower : MonoBehaviour
{
    private Transform target;

    [Header("Attributes")]
    public float range = 3.5f; //range from bottom of tower
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Misc")]
    private string enemyTag = "Enemy";
    private Vector3 enemyOffset = new Vector3(0.5f, 1.25f, 0.5f); //enemy position is bottom left of their model due to grid
    public Transform cannonStand, cannonBarrel, bottomOfTower, effectLocation; //tower parts
    public float turnSpeed = 7f;
    public GameObject shootEffect;

    public GameObject bulletPrefab;
    //public Transform firePoint;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f); //checks every half second, to reduce load of system
    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDist = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies) //find enemy closest
        {
            float distToEnemy = Vector3.Distance(bottomOfTower.position, enemy.transform.position); //get distance to all enemies
            if (distToEnemy < shortestDist) //find the shortest distance to enemy and said enemy
            {
                shortestDist = distToEnemy;
                nearestEnemy = enemy; //sets the it as nearest enemy
            }
        }

        if (nearestEnemy != null && shortestDist <= range) //if closest enemy is in range
        {
            target = nearestEnemy.transform;
        }
        else //finds new enemy if not in range
        {
            target = null;
        }
    }

    private void Update()
    {
        if (target == null) return; //nothing to do if no target


        Vector3 dir = target.position - cannonBarrel.position + enemyOffset;
        Quaternion standQuatRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(cannonBarrel.rotation, standQuatRotation, turnSpeed * Time.deltaTime).eulerAngles;
        cannonStand.rotation = Quaternion.Euler(0f, rotation.y, 0f); //rotates stand to turn left and right
        cannonBarrel.localRotation = Quaternion.Euler(rotation.x, 0f, 0f); //rotates barrel to turn up and down

        if (fireCountdown <= 0f) //shoots
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    private void Shoot()
    {
        GameObject bulletObject = Instantiate(bulletPrefab, cannonBarrel.position, cannonBarrel.rotation);
        GameObject effectObject = Instantiate(shootEffect, effectLocation.position, effectLocation.rotation); //particle effect for shooting
        Destroy(effectObject, 2f);
        Bullet bullet = bulletObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target); //bullet goes to target
        }
    }

    private void OnDrawGizmosSelected() //can see range in editor
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(bottomOfTower.position, range);
    }
}
