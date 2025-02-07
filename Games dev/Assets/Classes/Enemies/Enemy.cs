using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Apple;

public class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    private Vector3 _destination;
    public float maxHP;
    public float currentHP;
    public float Speed;
    public int ID;

    public void Init()
    {
        currentHP = maxHP;
        setDestToBase();
    }

    public void setDestToBase()
    {
        GameObject.FindWithTag("Base").transform.position = _destination; //finds the base via tag
        _agent.SetDestination(_destination); //set desination to the base
        //Debug.Log(_destination);
    }
}
