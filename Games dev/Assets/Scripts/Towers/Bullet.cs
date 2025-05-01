using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;

    public float speed = 40f;
    private Vector3 enemyOffset = new Vector3(0.5f, 1f, 0.5f);
    public void Seek(Transform target) //set target
    {
        this.target = target;
    }

    private void Update()
    {
        if (target == null) //no target then bullet dissapears
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position + enemyOffset; //enemy parent is bottom left hence the offset
        float distThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distThisFrame) //if the bullet hits
        {
            HitTarget();
            return;
        }
        //Debug.Log("hello");
        transform.Translate(dir.normalized * distThisFrame, Space.World); //else itll move closer to target
    }

    private void HitTarget() //for now just destroys itself
    {
        Destroy(gameObject);
    }
}
