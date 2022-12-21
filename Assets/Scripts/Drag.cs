using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    //Variables
    
    public Rigidbody2D rb;
    private Vector3 initialPos,worldPos,finalPos;
    private RaycastHit2D hit;
    public bool ballTouched;
    private float maxDistance = 5f;
    public float speed,distance;
    Vector2 direction;
    public Transform trajectory;
    private float xDistance;
    public GameObject arrow,powerObj;
    public LineRenderer lineRenderer;
    private Vector3 arrowInitialPos;
  

    // Start is called before the first frame update
    void Start()
    {          
        arrowInitialPos = arrow.transform.position;
        arrow.transform.GetChild(0).gameObject.SetActive(false);
        arrow.transform.GetChild(1).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        

        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        if (Input.GetMouseButtonDown(0))
        {            
            initialPos = worldPos;
            powerObj.transform.position = new Vector2(initialPos.x, initialPos.y);
            powerObj.SetActive(true);
        }
        if (Input.GetMouseButton(0))
        {
           
          if(!ballTouched)
            {
                Vector3 temPos = worldPos;
                if (temPos.x > initialPos.x)
                {
                    temPos.x = initialPos.x;
                }
                              
                xDistance= Mathf.Abs(Vector2.Distance(temPos,initialPos ));
                if(xDistance >= maxDistance)
                {
                    Debug.Log("More than maxdistance");
                    finalPos = initialPos + (temPos-initialPos).normalized * maxDistance;
                    
                }
                else
                {
                    finalPos = temPos;
                }
               
                direction = finalPos - initialPos;
                distance = Mathf.Clamp(Vector2.Distance(finalPos, initialPos), 0, maxDistance);                
                arrow.transform.position = arrowInitialPos + (new Vector3(direction.x, direction.y,0));
                arrow.transform.right = -direction;
                lineRenderer.SetPosition(1, new Vector3(direction.x, direction.y, 0) * 10f);
                 Vector3 velocityTemp= (new Vector2(direction.x, direction.y) * -speed * distance);
                EnableTrajectoryDots(velocityTemp);          

            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            arrow.transform.GetChild(0).gameObject.SetActive(true);
            arrow.transform.GetChild(1).gameObject.SetActive(false);
            powerObj.SetActive(false);
            distance = Mathf.Clamp(Vector2.Distance(finalPos, initialPos), 0, maxDistance);
            DisableTrajectoryDots();            
            rb.gravityScale = 1f;
            rb.velocity=(new Vector2(direction.x,direction.y) * -speed*distance);
            ballTouched= false;
        }
    }

    public Vector3 TrajectoryDotPosition( float elapsedTime,Vector3 finalVelocity)
    {
        return arrow.transform.position + finalVelocity * elapsedTime + 0.5f * Physics.gravity * elapsedTime * elapsedTime;
    }
    public void EnableTrajectoryDots(Vector3 velocity)
    {
        for (int i = 0; i < trajectory.childCount; i++)
        {
            
            trajectory.GetChild(i).transform.position = TrajectoryDotPosition(i * 0.1f, new Vector3(velocity.x,velocity.y,0));
            if(i>1)
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
