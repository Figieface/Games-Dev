using UnityEngine;



[CreateAssetMenu(fileName = "New EnemySummonData", menuName = "Create EnemySummonData")]
public class EnemySummonData : ScriptableObject
{
    public GameObject EnemyPrefab;
    public int EnemyId;
}
