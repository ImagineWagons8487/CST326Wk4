using System;
using UnityEngine;

public class QuestionBlockScript : MonoBehaviour
{
    private float t;
    private int score;
    private int coins;

    public delegate void PlayerHitQuestion(GameObject qBlock, int score, int coins);

    public static event PlayerHitQuestion OnPlayerHitQuestion;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        t = 0;
        score = 100;
        coins = 1;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t >= .15f && gameObject.CompareTag("QuestionBlock"))
        {
            Vector2 offset = GetComponent<Renderer>().material.mainTextureOffset;
            offset = new Vector2(offset.x, offset.y - .2f);
            if (offset.y == 1f)
            {
                offset.y = 0;
            }
            GetComponent<Renderer>().material.mainTextureOffset = offset;
            t = 0;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Mario") && 
            (other.transform.position.y + other.gameObject.GetComponent<Collider>().bounds.extents.y*2f) < transform.position.y &&
            Math.Abs(other.transform.position.x - transform.position.x) < GetComponent<Collider>().bounds.extents.x-.2f)
        {
            OnPlayerHitQuestion?.Invoke(gameObject, score, coins);
        }
    }
}
