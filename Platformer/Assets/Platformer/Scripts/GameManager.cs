using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera cam;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI pointText;
    public TextMeshProUGUI coinText;
    public GameObject coin;
    public Material disabledQuestionMaterial;
    public LayerMask lM;
    private int coinCount;
    

    private int points;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coinCount = 0;
        points = 0;
        coinText.text = $"x0{coinCount}";
    }

    // Update is called once per frame
    void Update()
    {
        int timeLeft = 300 - (int)Time.time;
        timerText.text = $"Time:\n {timeLeft}";

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenPos = Input.mousePosition;
            // Debug.Log(screenPos);
            Ray cursorRay = cam.ScreenPointToRay(screenPos);
            bool rayHitSomething = Physics.Raycast(cursorRay, out RaycastHit screenHitInfo, 100f, lM);
            // if (Physics.Raycast(cursorRay, out screenHitInfo, 100f))
            // {
            //     Debug.Log($"ScreenHitInfo.transform: {screenHitInfo.transform.position}");
            // }
            Debug.DrawRay(cursorRay.origin, cursorRay.direction*100f, Color.red, 2f);
            // Ray cursorRay = cam.ScreenPointToRay(screenPos);
            // bool rayHitSomething = Physics.Raycast(cursorRay, out RaycastHit screenHitInfo);
            Debug.Log($"Ray hit:{rayHitSomething}");
            if (rayHitSomething && screenHitInfo.transform.gameObject.CompareTag("Brick"))
            {
                Debug.Log("brick");
                // Destroy(screenHitInfo.transform.gameObject);
                // points += 10;
                // pointText.text = $"Points:\n{points}";
            }
            else if (rayHitSomething && screenHitInfo.transform.gameObject.CompareTag("QuestionBlock"))
            {
                Debug.Log("QB");
                // StartCoroutine(coinGetAnimation(screenHitInfo.transform));
                // ++coinCount;
                // if (coinCount < 10)
                // {
                //     coinText.text = $"x0{coinCount}";
                // }
                // else
                // {
                //     coinText.text = $"x{coinCount}";
                // }
                // screenHitInfo.transform.gameObject.GetComponent<Renderer>().material = disabledQuestionMaterial;
                // screenHitInfo.transform.gameObject.tag = "Stone";
            }
            else if (rayHitSomething)
            {
                Debug.Log("Ray hit something!");
            }
        }
    }

    IEnumerator coinGetAnimation(Transform startTransform)
    {
        GameObject newCoin = Instantiate(coin, startTransform); 
        float duration = .4f;
        float timeElapsed = 0f;
        float distanceUp = 1.5f;
        Vector3 originalPos = newCoin.transform.position;
        while (timeElapsed*2 < duration)
        {
            float percentageComplete = timeElapsed / duration;
            Vector3 pos = newCoin.transform.position;
            Debug.Log(pos);
            float newY = originalPos.y + (coinEase(percentageComplete) * distanceUp);
            newCoin.transform.position = new Vector3(pos.x, newY, pos.z);
            yield return null;
            timeElapsed += Time.deltaTime;
        }
        Destroy(newCoin);
        
    }

    float coinEase(float x)
    {
        return (float)(1 - Math.Pow((1f-x), 5));
    }
}
