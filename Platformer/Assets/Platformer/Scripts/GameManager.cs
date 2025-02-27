using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.Scripts
{
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

        private bool playerFail = false;

        private bool playerWin = false;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            coinCount = 0;
            points = 0;
            coinText.text = $"x0{coinCount}";
            LavaScript.OnPlayerHitLava += LavaOnOnPlayerHitLava;
            QuestionBlockScript.OnPlayerHitQuestion += QuestionBlockOnOnPlayerHitQuestion;
            BrickScript.OnPlayerHitBrick += BrickBlockOnOnPlayerHitBrick;
            GoalScript.OnPlayerHitGoal += GoalScriptOnOnPlayerHitGoal;
        }


        // Update is called once per frame
        void Update()
        {
            int timeLeft = 0;
            if (!playerFail && !playerWin)
                timeLeft = 100 - (int)Time.time;
            if (timeLeft == 0 && !playerFail && !playerWin)
            {
                Debug.Log("Player Failed.");
                timerText.text = "Time:\n 0";
                playerFail = true;
            }
            else if(timeLeft > 0)
            {
                timerText.text = $"Time:\n {timeLeft}";
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 screenPos = Input.mousePosition;
                Ray cursorRay = cam.ScreenPointToRay(screenPos);
                bool rayHitSomething = Physics.Raycast(cursorRay, out RaycastHit screenHitInfo, 100f);
                if (rayHitSomething && screenHitInfo.transform.gameObject.CompareTag("Brick"))
                {
                    Destroy(screenHitInfo.transform.gameObject);
                    points += 100;
                    pointText.text = $"Points:\n{points}";
                }
                else if (rayHitSomething && screenHitInfo.transform.gameObject.CompareTag("QuestionBlock"))
                {
                    StartCoroutine(coinGetAnimation(screenHitInfo.transform));
                    ++coinCount;
                    points += 100;  
                    if (coinCount < 10)
                    {
                        coinText.text = $"x0{coinCount}";
                    }
                    else
                    {
                        coinText.text = $"x{coinCount}";
                    }
                    screenHitInfo.transform.gameObject.GetComponent<Renderer>().material = disabledQuestionMaterial;
                    screenHitInfo.transform.gameObject.tag = "Stone";
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

        void LavaOnOnPlayerHitLava()
        {
            if (!playerFail && !playerWin)
            {
                playerFail = true;
                Debug.Log("Player Failed.");
                timerText.text = "Time:\n 0";
            }
        }

        void QuestionBlockOnOnPlayerHitQuestion(GameObject qBlock, int score, int coins)
        {
            if(qBlock.CompareTag("QuestionBlock"))
            {
                StartCoroutine(coinGetAnimation(qBlock.transform));
                coinCount += coins;
                points += score;
                pointText.text = $"Points:\n{points}";
                if (coinCount < 10)
                {
                    coinText.text = $"x0{coinCount}";
                }
                else
                {
                    coinText.text = $"x{coinCount}";
                }

                qBlock.GetComponent<Renderer>().material = disabledQuestionMaterial;
                qBlock.transform.gameObject.tag = "Stone";
            }
        }

        void BrickBlockOnOnPlayerHitBrick(GameObject brick, int score)
        {
            Destroy(brick);
            points += 100;
            pointText.text = $"Points:\n{points}";
        }
        
        private void GoalScriptOnOnPlayerHitGoal()
        {
            if (!playerWin && !playerFail)
            {
                playerWin = true;
                Debug.Log("Player Won!");
            }
        }
    }
}
