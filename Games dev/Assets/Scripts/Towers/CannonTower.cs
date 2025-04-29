using UnityEngine;

public class CannonTower : MonoBehaviour
{
    private Transform target; //might be better to change this to vec3 if needed when doing damage script
    private float range = 7f; //3.7f
    public string enemyTag = "Enemy";
    private Vector3 enemyOffset = new Vector3(0.5f, 1.25f, 0.5f); //enemy position is bottom left of their model due to grid
    public Transform cannonStand, cannonBarrel;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDist = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distToEnemy = Vector3.Distance(cannonBarrel.position, enemy.transform.position); //get distance to all enemies
            if (distToEnemy < shortestDist) //find the shortest distance to enemy and said enemy
            {
                shortestDist = distToEnemy;
                nearestEnemy = enemy; //sets the it as nearest enemy
            }
        }

        if (nearestEnemy != null && shortestDist <= range)
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
        if (target == null) return;


        Vector3 dir = target.position - cannonBarrel.position + enemyOffset;
        Quaternion standQuatRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = standQuatRotation.eulerAngles;
        cannonStand.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        cannonBarrel.localRotation = Quaternion.Euler(rotation.x, 0f, 0f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(cannonBarrel.position, range);
    }
}
