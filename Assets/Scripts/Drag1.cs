using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag1 : MonoBehaviour
{

    public GameObject arrow;
    public Rigidbody2D rb;
    private Vector3 initialPos, worldPos, finalPos;
    private RaycastHit hit;
    public bool ballTouched;
    private float maxDistance = 1.2f;
    public float speed, distance;
    Vector2 direction;
    public Transform trajectory;
    private float xDistance;
    private Vector3 arrowInitialPos;

    private float arrowPosZ,camPosZ;

    // Start is called before the first frame update
    void Start()
    {
        arrowPosZ = arrow.transform.parent.transform.position.z;
        camPosZ = Camera.main.transform.position.z;
        arrowInitialPos = arrow.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, arrowPosZ + camPosZ+10f));
        if (Input.GetMouseButtonDown(0))
        {
            initialPos = worldPos;
            Debug.Log(initialPos + "intialpos");
           //if( Physics.Raycast()

        }
        if (Input.GetMouseButton(0))
        {

            if (!ballTouched)
            {
                Vector3 temPos = worldPos;
                //xDistance= temPos.x-initialPos.x;
                xDistance = Mathf.Abs(Vector2.Distance(temPos, initialPos));
                if (xDistance >= maxDistance)
                {
                    Debug.Log("More than maxdistance");
                    finalPos = initialPos + (temPos - initialPos).normalized * maxDistance;

                }
                else
                {
                    finalPos = temPos;
                }
                // Debug.Log(finalPos.x + "  finalPos");
                direction = finalPos - initialPos;
                distance = Mathf.Clamp(Vector2.Distance(finalPos, initialPos), 0, maxDistance);

                arrow.transform.position = new Vector3(finalPos.x, finalPos.y, 0);
                // arrow.transform.position = arrowInitialPos + new Vector3(finalPos.x, finalPos.y, 0);
                Vector3 velocityTemp = (new Vector2(direction.x, direction.y) * -speed * distance);

                EnableTrajectoryDots(velocityTemp);
                //EnableTrajectoryDots(new Vector3(velocityTemp.x,velocityTemp.y,arrowPosZ));

            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("arrowPos  " + arrow.transform.position);
            Debug.Log("finalPos  " + finalPos);
            /*finalPos= worldPos;
            direction= finalPos- initialPos;*/
            distance = Mathf.Clamp(Vector2.Distance(finalPos, initialPos), 0, maxDistance);

            DisableTrajectoryDots();
            rb.gravityScale = 1f;
            rb.velocity = (new Vector2(direction.x, direction.y) * -speed * distance);
            ballTouched = false;
        }
    }

    public Vector3 TrajectoryDotPosition(float elapsedTime, Vector3 finalVelocity)
    {
        return arrow.transform.position + finalVelocity * elapsedTime + 0.5f * Physics.gravity * elapsedTime * elapsedTime;
    }
    public void EnableTrajectoryDots(Vector3 velocity)
    {
        for (int i = 0; i < trajectory.childCount; i++)
        {
            trajectory.GetChild(i).transform.position = TrajectoryDotPosition(i * 0.1f, new Vector3(velocity.x, velocity.y, 0));
            trajectory.GetChild(i).gameObject.SetActive(true);
        }
    }
    public void DisableTrajectoryDots()
    {
        for (int i = 0; i < trajectory.childCount; i++)
        {

            trajectory.GetChild(i).gameObject.SetActive(false);
        }
    }
    /* private void OnCollisionEnter2D(Collision2D collision)
     {
         if (collision.gameObject.tag == "ground")
         {
             transform.position = initialPos;
             rb.gravityScale = 0;
             rb.velocity= Vector2.zero;
             rb.angularVelocity = 0f;
         }
     }*/
}
