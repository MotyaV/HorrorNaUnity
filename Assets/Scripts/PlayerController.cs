using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float headBobStrength = 0.1f;
    public AudioClip stepSound;
    public float stepDistance = 0.5f;
    
    private CharacterController characterController;
    private Camera mainCamera;
    private AudioSource audioSource;
    private Vector3 previousPosition;
    private float distanceTraveled;

    private float headBobTimer = 0f;
    private bool isHeadBobUp = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = stepSound;
        previousPosition = transform.position;
    }

    void Update()
    {
        MovePlayer();
        HeadBobEffect();
        PlayStepSound();
    }

    void MovePlayer()
    {
        float moveDirectionY = characterController.velocity.y;

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = transform.TransformDirection(move) * moveSpeed;

        move.y = moveDirectionY;

        characterController.Move(move * Time.deltaTime);

        distanceTraveled += Vector3.Distance(transform.position, previousPosition);
        previousPosition = transform.position;
    }

    void HeadBobEffect()
    {
        if (characterController.velocity.magnitude > 0.1f)
        {
            headBobTimer += Time.deltaTime * (isHeadBobUp ? 1 : -1);

            if (headBobTimer >= headBobStrength)
            {
                isHeadBobUp = false;
            }
            else if (headBobTimer <= -headBobStrength)
            {
                isHeadBobUp = true;
            }

            Vector3 cameraPosition = mainCamera.transform.localPosition;
            cameraPosition.y = Mathf.Lerp(cameraPosition.y, headBobTimer, Time.deltaTime * 10f);
            mainCamera.transform.localPosition = cameraPosition;
        }
    }

    void PlayStepSound()
    {
        if (distanceTraveled >= stepDistance)
        {
            audioSource.Play();
            distanceTraveled = 0f;
        }
    }
}
