using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float headBobStrength = 0.1f;
    public AudioClip[] stepSounds;
    public AudioClip jumpSound;
    public float stepDistance = 0.5f;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundMask;

    public float maxHP = 100f;
    public float currentHP;
    public float healingRate = 1.5f;
    private float lastDamageTime;
    public float maxSunResistance = 10f;
    public float currentSunResistance;
    public float sunResistanceDepletionRate = 2f;
    public float sunResistanceRegenRate = 2f;
    private bool isInShadow;
    public GameObject sun;
    public LayerMask raycastIgnoreMask;

    private Rigidbody rb;
    private AudioSource audioSource;
    private Vector3 previousPosition;
    private float distanceTraveled;
    private bool isGrounded;

    private float headBobTimer = 0f;
    private bool isHeadBobUp = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = gameObject.AddComponent<AudioSource>();
        previousPosition = transform.position;

        rb.drag = 5f;
        rb.freezeRotation = true;

        currentHP = maxHP;
        currentSunResistance = maxSunResistance;
        lastDamageTime = Time.time;
    }

    void Update()
    {
        MovePlayer();
        PlayStepSound();

        HPManagement();
        SunResistanceManagement();
    }

    void FixedUpdate()
    {
        HeadBobEffect();
    }

    void MovePlayer()
    {
        GroundCheck();

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveHorizontal + transform.forward * moveVertical;
        move = move.normalized * moveSpeed;

        Vector3 newVelocity = new Vector3(move.x, rb.velocity.y, move.z);
        rb.velocity = newVelocity;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            audioSource.PlayOneShot(jumpSound);
        }

        distanceTraveled += Vector3.Distance(transform.position, previousPosition);
        previousPosition = transform.position;
    }

    void HeadBobEffect()
    {
        if (rb.velocity.magnitude > 0.1f && isGrounded)
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

            Vector3 cameraPosition = Camera.main.transform.localPosition;
            cameraPosition.y = Mathf.Lerp(cameraPosition.y, headBobTimer, Time.deltaTime * 10f);
            Camera.main.transform.localPosition = cameraPosition;
        }
    }

    void PlayStepSound()
    {
        if (distanceTraveled >= stepDistance && isGrounded)
        {
            if (stepSounds.Length > 0)
            {
                audioSource.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)]);
            }
            distanceTraveled = 0f;
        }
    }

    void GroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);
    }

    void HPManagement()
    {
        if (Time.time - lastDamageTime >= 10f && currentHP < maxHP)
        {
            currentHP += healingRate * Time.deltaTime;
            currentHP = Mathf.Clamp(currentHP, 0f, maxHP);
        }
    }

    void SunResistanceManagement()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, sun.transform.position - transform.position, out hit, Mathf.Infinity, ~raycastIgnoreMask))
        {
            if (hit.collider.gameObject == sun)
            {
                isInShadow = false;
                currentSunResistance -= sunResistanceDepletionRate * Time.deltaTime;

                if (currentSunResistance <= 0)
                {
                    GameOver();
                }
            }
            else
            {
                isInShadow = true;
                currentSunResistance += sunResistanceRegenRate * Time.deltaTime;
                currentSunResistance = Mathf.Clamp(currentSunResistance, 0f, maxSunResistance);
            }
        }
        else
        {
            isInShadow = true;
            currentSunResistance += sunResistanceRegenRate * Time.deltaTime;
            currentSunResistance = Mathf.Clamp(currentSunResistance, 0f, maxSunResistance);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shadow"))
        {
            isInShadow = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Shadow"))
        {
            isInShadow = false;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        lastDamageTime = Time.time;
        if (currentHP <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}