using System;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class MageTower : MonoBehaviour
{
    private Transform target;

    [Header("Attributes")]
    public float range = 3.5f; //range from bottom of tower
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Misc")]
    private string enemyTag = "Enemy";
    private Vector3 enemyOffset = new Vector3(0.5f, 1.25f, 0.5f); //enemy position is bottom left of their model due to grid
    public Transform  Barrel, bottomOfTower, effectLocation; //tower parts
    public float turnSpeed = 7f;
    public GameObject bulletPrefab;
    public int bulletDamage;
    private Vector3 startBallPos;

    [Header("Upgrade")]
    public GameObject upgrade1;
    public GameObject upgrade2;

    //public Transform firePoint;

    private void Start()
    {
        startBallPos = Barrel.localPosition;
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
        float moveY = Mathf.Sin(Time.time * 2) * 0.2f;
        Barrel.localPosition = startBallPos + new Vector3(0f, moveY, 0f); //making it float up and down using a sin wave

        if (target == null) return; //nothing to do if no target

        if (fireCountdown <= 0f) //shoots
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    private void Shoot()
    {
        GameObject bulletObject = Instantiate(bulletPrefab, Barrel.position, Barrel.rotation);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.damage = bulletDamage;

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

    public void Upgrade1Chosen() //button listenr from  this
    {
        UpgradeTo(upgrade1);
    }

    public void Upgrade2Chosen() //button listenr from  this
    {
        UpgradeTo(upgrade2);
    }


    public void UpgradeTo(GameObject upgradeSelected)
    {
        if (upgradeSelected == null || StructureShop.currency < 100) //if you dont have enough it just returns
        {
            Debug.LogError("No upgraded prefab assigned!");
            return;
        }
        StructureShop.currency -= 100; //hard coded upgrade cost for now
        Vector3 position = transform.position; //saving positions
        Quaternion rotation = transform.rotation;
        Transform parent = transform.parent; //has to be in a parent so the tile system doesnt get confused that its been replaced
        Destroy(gameObject);
        // Instantiate the upgraded version
        GameObject newTower = Instantiate(upgradeSelected, position, rotation, parent);

        PlacementSystemm placementSystem = FindFirstObjectByType<PlacementSystemm>();
        Button[] allButtons = newTower.GetComponentsInChildren<Button>(true); // true = include inactive
        foreach (Button btn in allButtons)//getting sell button for new struture
        {
            if (btn.name == "Sell")
            {
                Button sellButton = btn;
                //Debug.Log(sellButton);
                sellButton.onClick.AddListener(() => placementSystem.SellTower());
                break;
            }
        }
    }
}
