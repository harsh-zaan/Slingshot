using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNShoot : MonoBehaviour
{
   
        private bool isPressed;
        private float releaseDelay;
        private float maxDragDistance = 1f;

        private Rigidbody rb;
        private SpringJoint2D sj;
        private Rigidbody2D slingRb;
        private LineRenderer lr;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            sj = GetComponent<SpringJoint2D>();
            slingRb = sj.connectedBody;
            lr = GetComponent<LineRenderer>();

            lr.enabled = true;

            releaseDelay = 1 / (sj.frequency * 4);
        }

        void Update()
        {
            if (isPressed)
            {
                DragArrow();
            }
        }

        private void DragArrow()
        {
            SetLineRendererPositions();

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float distance = Vector2.Distance(mousePosition, slingRb.position);

            if (distance > maxDragDistance)
            {
                Vector2 direction = (mousePosition - slingRb.position).normalized;
                rb.position = slingRb.position + direction * maxDragDistance;
            }
            else
            {
                rb.position = mousePosition;
            }
        }

        private void SetLineRendererPositions()
        {
            Vector3[] positions = new Vector3[2];
            positions[0] = rb.position;
            positions[1] = slingRb.position;
            lr.SetPositions(positions);
        }

        private void OnMouseDown()
        {
            isPressed = true;
            rb.isKinematic = true;
            lr.enabled = true;
        }

        private void OnMouseUp()
        {
            isPressed = false;
            rb.isKinematic = false;
            StartCoroutine(Release());
            lr.enabled = false;
        }

        private IEnumerator Release()
        {
            yield return new WaitForSeconds(releaseDelay);
            sj.enabled = false;
        }
}



