using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bow : MonoBehaviourPun
{
    public GameObject arrow;
    public float launchForce;
    public Transform shotPoint;
    public Animator anim;

    public GameObject point;
    GameObject[] points;
    public int numberOfPoints;
    public float spaceBetweenPoints;
    Vector2 direction;

    public Transform trajectory;

    public bool canShoot;

    //Variables
    public Rigidbody2D rb;
    private Vector3 initialPos, worldPos, finalPos;
    public bool ballTouched;
    private float maxDistance = 1.2f;
    public float speed, distance;
    private float xDistance;


    private void Start()
    {
        points = new GameObject[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i] = Instantiate(point, shotPoint.position, Quaternion.identity);
        }
    }
    void Update()
    {
     if(canShoot)
        {
            worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            //CreatePointsProjection();
            //if (Input.GetMouseButtonDown(0))
            //{

            //    // starts the first player's turn
            //    photonView.RPC("Shoot", RpcTarget.AllBuffered);

            //}
            if (Input.GetMouseButtonDown(0))
            {
                initialPos = worldPos;
            }
            if (Input.GetMouseButton(0))
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
				//transform.position = new Vector2(finalPos.x, finalPos.y);
				Vector3 velocityTemp = (new Vector3(direction.x, direction.y, 0) * -speed * distance);


				EnableTrajectoryDots(velocityTemp);
                
            }
            if (Input.GetMouseButtonUp(0))
            {
                /*finalPos= worldPos;
                direction= finalPos- initialPos;*/
                distance = Mathf.Clamp(Vector2.Distance(finalPos, initialPos), 0, maxDistance);

                DisableTrajectoryDots();
                //rb.gravityScale = 1f;
                //rb.velocity = (new Vector2(direction.x, direction.y) * -speed * distance);
                //GameObject newArrow = PhotonNetwork.Instantiate("Sphere7", shotPoint.position, shotPoint.rotation);
                GameObject newArrow = Instantiate(arrow, shotPoint.position, shotPoint.rotation);
                newArrow.GetComponent<Rigidbody2D>().velocity = (new Vector2(direction.x, direction.y) * -speed * distance);
                newArrow.GetComponent<Rigidbody2D>().gravityScale = 1f;
            }
        }

    }

    //void CreatePointsProjection()
    //{
    //    Vector2 bowPosition = transform.position;
    //    var mousePos = Input.mousePosition;

    //    mousePos.z = 100;
    //    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(mousePos);

    //    direction = mousePosition - bowPosition;

    //    transform.right = -1 * direction;


    //    for (int i = 0; i < numberOfPoints; i++)
    //    {
    //        points[i].transform.position = PointPosition(i * spaceBetweenPoints);
    //    }
    //}

    //[PunRPC]
    //void Shoot()
    //{
    //    Debug.Log("Shot");
    //    anim.SetTrigger("shootTrigger");
    //    GameObject newArrow = PhotonNetwork.Instantiate("Sphere7", shotPoint.position, shotPoint.rotation);
    //    newArrow.GetComponent<Rigidbody>().velocity = -1 * transform.right * launchForce;

    //}

    //Vector2 PointPosition(float t)
    //{
    //    Vector2 position = (Vector2)shotPoint.position + (direction.normalized * launchForce * t) + 0.5f * Physics2D.gravity * (t * t);
    //    return position;
    //}

    public Vector3 TrajectoryDotPosition(float elapsedTime, Vector3 finalVelocity)
    {
        return transform.position + finalVelocity * elapsedTime + 0.5f * Physics.gravity * elapsedTime * elapsedTime;
    }
    public void EnableTrajectoryDots(Vector3 velocity)
    {
        for (int i = 0; i < trajectory.childCount; i++)
        {
            trajectory.GetChild(i).transform.position = TrajectoryDotPosition(i * 0.1f, velocity);
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


}
