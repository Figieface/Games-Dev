using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePuddle : MonoBehaviour
{
    private List<GameObject> enemiesTouchingPuddle = new();

    private void Start()
    {
        InvokeRepeating("AreaDamage", 0f, 0.5f);
    }

    private void AreaDamage()
    {
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, 1);
        foreach (Collider collider in collidersInRange)
        {
            if (collider.tag == "Enemy")
            {
                collider.gameObject.GetComponent<EnemyMovement>().currentHP -= 5;
            }
        }

    }
}
