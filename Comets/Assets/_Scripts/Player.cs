﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {


    [Header("Player Movement")]
    [SerializeField] Rigidbody2D playerRigid;
    [SerializeField] float yVelocity;
    [SerializeField] float xRotation;
    private float thrustInput;
    private float turnInput;

    [Header("Screen Wrapping")]
    [SerializeField] float screenHeight;
    [SerializeField] float screenWidth;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        PlayerMovement();
        ScreenWrap();
	}

    private void ScreenWrap()
    {
        Vector2 playerPos = transform.position;

        if (transform.position.y > screenHeight)
        {
            playerPos.y = -screenHeight;
        }
        if (transform.position.y < -screenHeight)
        {
            playerPos.y = screenHeight;
        }
        if (transform.position.x > screenWidth)
        {
            playerPos.x = -screenWidth;
        }
        if (transform.position.x < -screenWidth)
        {
            playerPos.x = screenWidth;
        }
        transform.position = playerPos;
    }

    private void PlayerMovement()
    {
        thrustInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        playerRigid.AddRelativeForce(Vector2.up * thrustInput * yVelocity * Time.deltaTime);
        playerRigid.AddTorque(-turnInput * xRotation * Time.deltaTime);
    }
}