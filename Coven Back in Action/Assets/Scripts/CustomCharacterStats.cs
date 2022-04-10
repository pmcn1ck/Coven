using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class CustomCharacterStats : MonoBehaviour
{
    public string monsters;
    public int hp;
    int currentHP;
    public float movementSpeed;
    public int damage = 10;
    public int score = 100;


    NavMeshAgent agent;
    GameObject playerReference;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = hp;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;
        playerReference = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("FollowPlayer", 0f, 0.5f);

    }

    public void TakeHP(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
            Ded();
    }
    void FollowPlayer()
    {
        agent.SetDestination(playerReference.transform.position);
    }
    public void Ded()
    {
        Destroy(gameObject);
    }
}
