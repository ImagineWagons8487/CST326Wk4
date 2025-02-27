using System;
using UnityEngine;

public class BrickScript : MonoBehaviour
{
    public int score;
    public delegate void PlayerHitBrick(GameObject brick, int score);

    public static event PlayerHitBrick OnPlayerHitBrick;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Mario") &&
            (other.transform.position.y + other.gameObject.GetComponent<Collider>().bounds.extents.y * 2f) <
            transform.position.y &&
            Math.Abs(other.transform.position.x - transform.position.x) < GetComponent<Collider>().bounds.extents.x-.2f)
        {
            OnPlayerHitBrick?.Invoke(gameObject, score);
        }
    }
}
