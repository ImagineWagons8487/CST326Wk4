using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class DemoLogic : MonoBehaviour
{
    public GameObject parachuteObj;
    public GameObject packageObj;
    public Transform parachutePivot;
    public Transform debugSphereTransform;
    public Camera cam;
    public float parachuteOpenDistance = 5f;
    public float chuteOpenDuration;

    public float parachuteDrag = 7f;

    private float defaultParachuteDrag;

    private bool hasParachuteOpened = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultParachuteDrag = packageObj.GetComponent<Rigidbody>().linearDamping;
        parachuteObj.SetActive(false);

        StartCoroutine("TestCoroutine");
    }

    IEnumerator TestCoroutine()
    {
        Debug.Log("Started Coroutine");
        yield return new WaitForSeconds(1f); //waits for 1 second and then continues
        Debug.Log("doing the next thing");
        yield return new WaitForSeconds(1f);
        Debug.Log("doing the last thing");
    }

    // Update is called once per frame
    void Update()
    {
        Ray lookForGroundRay = new Ray(packageObj.transform.position, Vector3.down);
        if (Physics.Raycast(lookForGroundRay, out RaycastHit hitInfo))
        {
            bool chuteOpen = hitInfo.distance < parachuteOpenDistance && hitInfo.distance > 0.01f;
            Color lineColor = chuteOpen ? Color.red : Color.blue;
            Debug.DrawRay(packageObj.transform.position, Vector3.down * parachuteOpenDistance, lineColor);
            
            //use parachute drag
            packageObj.gameObject.GetComponent<Rigidbody>().linearDamping = chuteOpen ? parachuteDrag : defaultParachuteDrag;
            parachuteObj.SetActive(chuteOpen);

            if (chuteOpen && !hasParachuteOpened)
            {
                StartCoroutine(AnimateParachuteOpen());
                hasParachuteOpened = true;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenPos = Input.mousePosition;
            
            // create a ray that goes through the screenPos using a camera
            Ray cursorRay = cam.ScreenPointToRay(screenPos);
            bool rayHitSomething = Physics.Raycast(cursorRay, out RaycastHit screenHitInfo);
            if (rayHitSomething && screenHitInfo.transform.gameObject.CompareTag("Brick"))
            {
                debugSphereTransform.position = screenHitInfo.point;
            }
        }
    }

    IEnumerator AnimateParachuteOpen()
    {
        float timeElapsed = 0f;
        parachutePivot.localScale = Vector3.zero;

        while (timeElapsed < chuteOpenDuration)
        {
            float percentComplete = timeElapsed / chuteOpenDuration;
            parachutePivot.localScale = new Vector3(percentComplete, percentComplete, percentComplete);
            Debug.Log(percentComplete);

            yield return null; // waits until next frame
            timeElapsed += Time.deltaTime;
        }
    }
}
