using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowhero : MonoBehaviour
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
       
        {
            Vector2 bowPosition = transform.position;
            var mousePos = Input.mousePosition;

            mousePos.z = 100;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(mousePos);
            Debug.Log(mousePosition);

            direction = mousePosition - bowPosition;

            transform.right = -1 * direction;

            if (Input.GetMouseButtonDown(0))
            {
               
                Shoot();

            }
            for (int i = 0; i < numberOfPoints; i++)
            {
                points[i].transform.position = PointPosition(i * spaceBetweenPoints);
            }

            void Shoot()
            {
                anim.SetTrigger("shootTrigger");
                GameObject newArrow = Instantiate(arrow, shotPoint.position, shotPoint.rotation);
                newArrow.GetComponent<Rigidbody>().velocity = -1 * transform.right * launchForce;
               
            }

            Vector2 PointPosition(float t)
            {
                Vector2 position = (Vector2)shotPoint.position + (direction.normalized * launchForce * t) + 0.5f * Physics2D.gravity * (t * t);
                return position;
            }
        }
    }
}
