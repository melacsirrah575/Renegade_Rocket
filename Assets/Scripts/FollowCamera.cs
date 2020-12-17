using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
  public Transform target;

  private Vector3 cameraOffSet;

[Range(0.01f, 1.0f)]
  public float smoothFactor = 0.5f;
 
    void Start()
    {
        cameraOffSet = transform.position - target.position;
    }

     void LateUpdate()
     {
         Vector3 newPos = target.position + cameraOffSet;

         transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);

     }
}
