
using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu]
public class EnemyDBSO : ScriptableObject
{
    public List<EnemyData> enemyData;
}

[Serializable]
public class EnemyData
{ //SO used to store data on enemies
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public int MaxHP { get; private set; }
    [field: SerializeField] public int MoveSpeed { get; private set; }
    [field: SerializeField] public GameObject Prefab { get; private set; }
}
