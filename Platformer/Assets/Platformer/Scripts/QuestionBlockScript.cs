using UnityEngine;

public class QuestionBlockScript : MonoBehaviour
{
    private float t;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        t = 0;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t >= .15f)
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
}
