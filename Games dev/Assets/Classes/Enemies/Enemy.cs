using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHP;
    public float currentHP;
    public float Speed;
    public int ID;

    public void Init()
    {
        currentHP = maxHP;
    }
}
