using System;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    public delegate void PlayerHitGoal();

    public static event PlayerHitGoal OnPlayerHitGoal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Mario"))
            OnPlayerHitGoal?.Invoke();
    }
}
