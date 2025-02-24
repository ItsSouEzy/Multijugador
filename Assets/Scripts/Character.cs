using Photon.Pun.Demo.Asteroids;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviourPun
{
    private Rigidbody rb;

    public float speed = 5f;

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");


            Vector3 movement = new Vector3(moveX, 0 ,moveZ) * speed;
            rb.velocity = movement;
            
        }
    }

   

}
