using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerenemy : MonoBehaviour
{
    public Animator ani;
    public ParticleSystem ps;
   
    void Start()
    {
        GetComponent<ParticleSystem>().Pause();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "blast")
        {

            ani.SetTrigger("hurtTrigger");
            Destroy(collision.gameObject);
            ParticleSystem.EmissionModule em = GetComponent<ParticleSystem>().emission;
            em.enabled = true;
            ps.Play();
       
        }

    }
}
