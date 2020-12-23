using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
   Rigidbody rigidBody;

//These values are the base values. Can be changed in Unity
   [SerializeField] float rcsThrust = 100f;
   [SerializeField] float mainThrust = 100f;
   [SerializeField] float levelLoadDelay = 2f;

    bool isTransitioning = false;
    bool collisionsDisabled = false;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>(); 
    }

    
    void Update()
    {
        if (!isTransitioning)
        {
        RespondToRotateInput();
        RespondToThrustInput();
        }

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

    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionsDisabled)
        {
            //ignores collisions when dead
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Death":
                StartDeathSequence();
                break;
            case "End":
                StartEndSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        // todo stop effects from happening
        isTransitioning = true;
        Invoke("LoadFirstLevel" , levelLoadDelay);
    }

    private void StartEndSequence()
    {
        // todo add effects for ending level
        isTransitioning = true;
        Invoke("LoadNextLevel" , levelLoadDelay);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
