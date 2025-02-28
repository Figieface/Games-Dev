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
    public bool endReached;

    public void Init()
    {
        endReached = false;
        currentHP = maxHP;
        setDestToBase();
    }

    private void LateUpdate()
    {
        setDestToBase();
    }

    public void setDestToBase()
    {
        _destination = GameObject.FindWithTag("Base").transform.position; //finds the base via tag
        Debug.Log(_destination);
        _agent.SetDestination(_destination); //set desination to the base
        //Debug.Log(_destination);
    }
}
