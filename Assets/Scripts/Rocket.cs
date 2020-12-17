using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
   Rigidbody rigidBody;

//These values are the base values. Can be changed in Unity
   [SerializeField] float rcsThrust = 100f;
   [SerializeField] float mainThrust = 100f;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>(); 
    }

    
    void Update()
    {
        RespondToRotateInput();
        RespondToThrustInput();
    }

    private void RespondToRotateInput()
    {
        //This removes the rotation due to physics
        rigidBody.angularVelocity = Vector3.zero;

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
    }

    private void RespondToThrustInput()
    {
        float thrustThisFrame = mainThrust *Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust(thrustThisFrame);
        }
        else
        {
            StopApplyingThrust();
        }
    }

    private void StopApplyingThrust()
    {
        //This is where audio and particle effects would be told to stop
    }

    private void ApplyThrust(float thrustThisFrame)
    {
        rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);

        //Will tell audio/particle effects to start here if given enough time
    }
}
