using UnityEngine;

public class BaseController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>(); // Check if the colliding object has the Enemy script
        if (enemy != null)
        {
            enemy.endReached = true;  // Set the boolean to true
            //Debug.Log(enemy.gameObject.name + " has reached the base!");
        }
    }
}
