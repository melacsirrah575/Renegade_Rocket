using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
   Rigidbody rigidBody;
   AudioSource audioSource;

//These values are the base values. Can be changed in Unity
   [SerializeField] float rcsThrust = 100f;
   [SerializeField] float mainThrust = 100f;
   [SerializeField] float levelLoadDelay = 2f;

   [SerializeField] AudioClip mainEngine;
   [SerializeField] AudioClip dying;
   [SerializeField] AudioClip endLevel;

    bool isTransitioning = false;
    bool collisionsDisabled = false;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>(); 
        audioSource = GetComponent<AudioSource>();
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
        audioSource.Stop();
    }

    private void ApplyThrust(float thrustThisFrame)
    {
        rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }

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
        audioSource.Stop();
        audioSource.PlayOneShot(dying);
        Invoke("LoadFirstLevel" , levelLoadDelay);
    }

    private void StartEndSequence()
    {
        // todo add effects for ending level
        audioSource.Stop();
        audioSource.PlayOneShot(endLevel);
        isTransitioning = true;
        Invoke("LoadNextLevel" , levelLoadDelay);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            //Loops back to beginning
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
